using UnityEngine;
using System.Collections;
using System.Collections.Generic;	// Dictionary

/// <summary>
/// Chaotic particles.
///
/// Evolve particles using a selected chaotic evolution equation. 
///
/// Evolution of the system is perfomed in "physics space" since each set of chaotic
/// equations generally works in co-ordinates center on the origin. The script then 
/// adjusts the world position and scale based on the position attributes of the 
/// game object the script is a component of. 
///
/// The initial positions are taken by taking the particle position relative to the
/// transform of the game object. Any particle velocities are set to zero - since the
/// evolution path is controlled by the chaotic system.
///
/// </summary>
[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(ParticleSystemRenderer))]
public class ChaoticParticles: ChaoticSystem  {

	private ParticleSystem particalSystem;

	// Initial position in world space
	private Vector3 initialPosition; 
	private ChaosEqn chaosEqn;

	// Define these as class variables so they can 
	// be alloc-ed once in Awake and not in every Evolve()
	// Internal physics position
	private float[,] x;
	// Runge-Kutta intermediate values - define globally to avoid alloc per update
	private float[] x_in;
	private float[] a_n;
	private float[] b_n;
	private float[] c_n;
	private float[] d_n;
	private float[] arg; 

	private ParticleSystem chaosParticles;
    private ParticleSystem.Particle[] particles;
	private int particleCount;

	/*
		Need to keep a parallel copy of the physics co-ordinates for each particle and handle
		particle creation/extinction. (In cases where e.g. scale by 0 to project system to plane
		there is no way to invert this back to a physics co-ordinate - hence the need to have a
		parallel copy in 3D for each particle.)

		As particles come and go they can shuffle in the particles[] array, so it becomes necessary
		to check their identities via their random seed and keep the parallel physics array in 
		alignment. 
	*/
	// per-particle activity
	private bool[] inactive;
	private bool playing; 

	private bool oneTimeBurst;

	private uint[] seed; 	// tracking array for random seed attached to each particle
	private Dictionary<uint,int> particleBySeed; 
	private int lastParticleCount; 

	private const float OUT_OF_VIEW = 10000f;

	// debug flag to monitor/understand the code
	private const bool debugLogs = false;
	private int debugCnt = 0; 

	void Awake() {

		chaosParticles = GetComponent<ParticleSystem>();

		ParticleSystemRenderer psr = GetComponent<ParticleSystemRenderer>();
		if (psr.renderMode == ParticleSystemRenderMode.Stretch) {
			Debug.LogError("Stretch render mode does not work for ChaoticParticles");
		}

		// init here so do not re-alloc each frame. Do not otherwise need to persist. 
		x_in = new float[3];
		a_n = new float[3];
		b_n = new float[3];
		c_n = new float[3];
		d_n = new float[3];
		arg = new float[3];

		Init();

		// determine if this is a one-time burst scenario
		int burstCount = 0; 
		ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[chaosParticles.emission.burstCount]; 
		chaosParticles.emission.GetBursts(bursts);
		foreach (ParticleSystem.Burst burst in bursts) {
			burstCount += burst.maxCount;
		}
		if (burstCount == chaosParticles.main.maxParticles) {
			Debug.Log("One time burst");
			oneTimeBurst = true;
		}
		particleBySeed = new Dictionary<uint,int>();
		InitParticleData();
	}

	public override void Init() {
		chaosEqn = ChaosEqnFactory.Create(selectedEqn, selectedParams, customParams);
		initialPosition = transform.position;
		TimeInit();
	}

	private void InitParticleData() {
		if (chaosParticles == null) {
			Debug.LogError("Must be attached to a particle system object");
			return;
		}
		// create array to hold particles
		particles = new ParticleSystem.Particle[chaosParticles.main.maxParticles]; 
		// get particles from the system (this fills in particles[])
		chaosParticles.GetParticles(particles);
		int maxParticles = chaosParticles.main.maxParticles;
		#pragma warning disable 162		// disable unreachable code warning
		if (debugLogs) {
			Debug.Log("Init numParticles=" + maxParticles);
		}
		#pragma warning restore 162
		x = new float[maxParticles,3];
		seed = new uint[maxParticles];
		inactive = new bool[maxParticles];
	}

	/// <summary>
	/// Inits the particles. The particles need to evolve in physics space - which is centered at 
	/// (0,0,0). Since the particle generator chosen will add some offset but provide a local 
	/// position.
	/// </summary>
	/// <param name="fromP">From particle number.</param>
	/// <param name="toP">To particle number.</param>
	private void InitParticles(int fromP, int toP) {
		for (int i=fromP; i < toP; i++) {
			x[i,0] = chaosEqn.paramBundle.initialPosition.x + particles[i].position.x - transform.position.x;
			x[i,1] = chaosEqn.paramBundle.initialPosition.y + particles[i].position.y - transform.position.y;
			x[i,2] = chaosEqn.paramBundle.initialPosition.z + particles[i].position.z - transform.position.z;
			inactive[i] = false;
			particles[i].velocity = Vector3.zero;
		}
		#pragma warning disable 162		// disable unreachable code warning
		if (debugLogs) {
			Debug.Log(string.Format("InitParticles from={0} to={1} pp_pos={2} phy_pos=({3},{4},{5})",
				fromP, toP, particles[fromP].position, x[fromP,0], x[fromP,1], x[fromP,2] ));
		}
		#pragma warning restore 162
	}

	//
	// Emmisive particle management
	// - per cycle look for new particles or cases where particles expired and were shuffled
	//

	void ParticleLifeCycleHandler()
	{
		// Particle life cycle management
		// - need GetParticles() call to get the correct number of active particle (p.particleCount did not work)
		// - IIUC this is a re-copy and it would be better to avoid this if possible
		particleCount = chaosParticles.GetParticles (particles);
		if (lastParticleCount < particleCount) {
			// there are new particles
			InitParticles(lastParticleCount, chaosParticles.particleCount);
			for (int i = lastParticleCount; i < chaosParticles.particleCount; i++) {
				inactive[i] = false;
				seed[i] = particles[i].randomSeed;
				particleBySeed[particles[i].randomSeed] = i;
			}
			lastParticleCount = chaosParticles.particleCount;
		}
		if (oneTimeBurst) {
			// not doing life cycle for this particle system
			return;
		}
		// Check if any existing particles were replaced. 
		// As particles expire, Unity will move particles from the end down into their slot and reduce
		// the number of active particles. Need to detect this and move their physics data.
		// This makes emmisive particle systems more CPU intensive. 
		for (int i = 0; i < chaosParticles.particleCount; i++) {
			if (seed[i] != particles[i].randomSeed) {
				#pragma warning disable 162		// disable unreachable code warning
				if (debugLogs)
					Debug.Log("Seed changed was:" + seed[i] + " now:" + particles[i].randomSeed);
				#pragma warning restore 162
				// particle has been replaced
				particleBySeed.Remove (seed [i]);
				// remove old seed from hash
				if (particleBySeed.ContainsKey (particles[i].randomSeed)) {
					// particle was moved - copy physical data down
					int oldIndex = particleBySeed[particles[i].randomSeed];
					x[i, 0] = x[oldIndex, 0];
					x[i, 1] = x[oldIndex, 1];
					x[i, 2] = x[oldIndex, 2];
					particleBySeed [particles[i].randomSeed] = i;
					#pragma warning disable 162		// disable unreachable code warning
					if (debugLogs)
						Debug.Log("Shuffling particle from " + oldIndex + " to " + i);
					#pragma warning restore 162
				}
				else {
					#pragma warning disable 162		// disable unreachable code warning
					if (debugLogs)
						Debug.Log("Reusing particle " + i + " vel=" + particles[i].velocity); 
					#pragma warning restore 162
					InitParticles(i, i+1);
					particleBySeed[particles[i].randomSeed] = i;
					#pragma warning disable 162		// disable unreachable code warning
					if (debugLogs)
						Debug.Log("Post-Setup Reusing particle " + i); 
					#pragma warning restore 162
				}
				seed[i] = particles[i].randomSeed;
				inactive[i] = false;
			}
		}
	}

	// Evolve the dynamical system using a 4th order RK integrator
	private void Evolve(float h) {
		// do nothing if all inactive
		if (inactive == null) {
			return; 	// Particle system has not init-ed yet or is done
		}
		//  (did not work in Start() -> Unity bug? Init sequencing?)
		if (!playing) {
			chaosParticles.Play();
			playing = true;
		}

		for (int j=0; j < particleCount; j++) {
			if (!inactive[j]) {

				Vector3 xout = Vector3.zero;
				x_in[0] = x[j,0];
				x_in[1] = x[j,1];
				x_in[2] = x[j,2];
				chaosEqn.Function(ref x_in, ref a_n);
				//Debug.Log("j=" + j + " x=" + x[j,0] + " arg0=" + arg[0] +  " a_n0=" + a_n[0]);
				float h_frac = h/2f;
				arg[0] = x[j,0] + h_frac * a_n[0];
				arg[1] = x[j,1] + h_frac * a_n[1];
				arg[2] = x[j,2] + h_frac * a_n[2];
				chaosEqn.Function(ref arg, ref b_n);
				arg[0] = x[j,0] + h_frac * b_n[0];
				arg[1] = x[j,1] + h_frac * b_n[1];
				arg[2] = x[j,2] + h_frac * b_n[2];
				chaosEqn.Function(ref arg, ref c_n);
				h_frac = h;
				arg[0] = x[j,0] + h_frac * c_n[0];
				arg[1] = x[j,1] + h_frac * c_n[1];
				arg[2] = x[j,2] + h_frac * c_n[2];
				chaosEqn.Function(ref arg, ref d_n);
				h_frac = h/6f;
				x[j,0] = x[j,0] + h_frac*(a_n[0] + 2f * b_n[0] + 2f * c_n[0] + d_n[0]);
				x[j,1] = x[j,1] + h_frac*(a_n[1] + 2f * b_n[1] + 2f * c_n[1] + d_n[1]);
				x[j,2] = x[j,2] + h_frac*(a_n[2] + 2f * b_n[2] + 2f * c_n[2] + d_n[2]);
				if (double.IsNaN(x[j,0]) || double.IsNaN(x[j,1]) || double.IsNaN(x[j,2])) {
					inactive[j] = true;
				}
			}
		}
	}

	void FixedUpdate() {

		// need a new copy of particles every frame (they age out, shuffle etc.)
        particles = new ParticleSystem.Particle[chaosParticles.main.maxParticles];
		ParticleLifeCycleHandler();

		int numSteps = CalcNumSteps();

		for (int i=0; i < numSteps; i++) {
			// position is in the scale of the dynamical system. Apply the 
			// transform scale
			Evolve(DT);
		}
		UpdateParticles();
	}


	/// <summary>
	/// Updates the particles positions in world space.
	///
	/// UpdateParticles is called from the GravityEngine. Do not call from other scripts. 
	/// </summary>
	public void UpdateParticles() {
		Vector3 phy_pos; 
		for (int i=0; i < lastParticleCount; i++) {
			if (!inactive[i]) {
				// particles are in physics space - map back to world space
				phy_pos = new Vector3((float) x[i,0], (float) x[i,1],(float) x[i,2]); 
				phy_pos = (phy_pos + chaosEqn.paramBundle.offset) * chaosEqn.paramBundle.scale;
				particles[i].position = Vector3.Scale(phy_pos, evolveScale) + initialPosition;
				particles[i].position = transform.rotation * particles[i].position;
			}
		}
		chaosParticles.SetParticles(particles, particleCount);
		#pragma warning disable 162, 429		// disable unreachable code warning
		if (debugLogs && debugCnt++ > 30) {
			debugCnt = 0; 
			string log = "time = " + Time.time + " remaining=" + particleCount ;
			log += " is Stopped " + chaosParticles.isStopped + " num=" + chaosParticles.particleCount + " pcount=" + particleCount + "\n";
			int logTo = 1; //(chaosParticles.main.maxParticles < 10) ? chaosParticles.main.maxParticles : 10;
			for (int i=0; i < logTo; i++) {
				log += string.Format("{0}  rand={1} life={2} inactive={3} ", i, particles[i].randomSeed, particles[i].remainingLifetime, inactive[i]);
				log += " pos=" + particles[i].position ;
				log += " phyPos= " + x[i,0] + " " + x[i,1] + " " + x[i,2];
				log += "\n";
			}
			Debug.Log(log);
		}
		#pragma warning restore 162, 429
	}

}
