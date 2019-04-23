using UnityEngine;
using System.Collections;

/// <summary>
/// Chaotic motion.
/// Evolve the user specified chaotic system with the parameters held by the
/// ChaoticSystem parent.
///
/// The script optionally handles a ChaosTrail - similar to a TrailRenderer but
/// optimized to ensure that interpolated positions are added to the path to 
/// ensure a user-specified "smoothness" is acheived. 
///
/// Evolution of the system is perfomed in "physics space" since each set of chaotic
/// equations generally works in co-ordinates center on the origin. The script then 
/// adjusts the world position and scale based on the transform attributes of the 
/// game object the script is a component of. 
/// </summary>
public class ChaoticMotion: ChaoticSystem  {

	// optional chaos trail renderer (better than TrailRenderer, since it 
	// adds intermediate points from integration to ensure smooth curve
	// at all speeds)
	private ChaosTrail chaosTrail; 

	private ChaosEqn chaosEqn;

	// Initial position in world space
	private Vector3 initialPosition; 
	private Vector3 lastTrail;


	// Define these as class variables so they can 
	// be alloc-ed once in Awake and not in every Evolve()
	// Internal physics position
	private float[] x;
	// Runge-Kutta intermediate values
	private float[] a_n;
	private float[] b_n;
	private float[] c_n;
	private float[] d_n;
	private float[] arg; 

	private bool diverged; 
	private bool paused; 

	void Awake() {
		Init();
	}

	/// <summary>
	/// Init the equation using the selected equation and parameter bundle. 
	/// </summary>
	public override void Init() {
		x = new float[3];
		a_n = new float[3];
		b_n = new float[3];
		c_n = new float[3];
		d_n = new float[3];
		arg = new float[3];

		chaosEqn = ChaosEqnFactory.Create(selectedEqn, selectedParams, customParams);
		initialPosition = transform.position;
		lastTrail = transform.position;

		StartAt(chaosEqn.paramBundle.initialPosition);

		// check for a ChaosTrail child
		chaosTrail = GetComponentInChildren<ChaosTrail>();

		paused = false;
		TimeInit();
	}

	private void StartAt(Vector3 xstart) {
		x[0] = xstart.x;
		x[1] = xstart.y;
		x[2] = xstart.z;
	}

	// Evolve the dynamical system using a 4th order RK integrator
	private Vector3 Evolve(float h) {

		Vector3 xout = Vector3.zero;
		chaosEqn.Function(ref x, ref a_n);
		float h_frac = h/2f;
		arg[0] = x[0] + h_frac * a_n[0];
		arg[1] = x[1] + h_frac * a_n[1];
		arg[2] = x[2] + h_frac * a_n[2];
		chaosEqn.Function(ref arg, ref b_n);
		arg[0] = x[0] + h_frac * b_n[0];
		arg[1] = x[1] + h_frac * b_n[1];
		arg[2] = x[2] + h_frac * b_n[2];
		chaosEqn.Function(ref arg, ref c_n);
		h_frac = h;
		arg[0] = x[0] + h_frac * c_n[0];
		arg[1] = x[1] + h_frac * c_n[1];
		arg[2] = x[2] + h_frac * c_n[2];
		chaosEqn.Function(ref arg, ref d_n);
		h_frac = h/6f;
		xout.x = x[0] + h_frac*(a_n[0] + 2f * b_n[0] + 2f * c_n[0] + d_n[0]);
		xout.y = x[1] + h_frac*(a_n[1] + 2f * b_n[1] + 2f * c_n[1] + d_n[1]);
		xout.z = x[2] + h_frac*(a_n[2] + 2f * b_n[2] + 2f * c_n[2] + d_n[2]);
		x[0] = xout.x;
		x[1] = xout.y;
		x[2] = xout.z;
		return xout;
	}

	void FixedUpdate() {

		if (diverged || paused) {
			return;
		}

		Vector3 position = transform.position;

		int numSteps = CalcNumSteps();

		for (int i=0; i < numSteps; i++) {
			// apply the parameter bundle offset and scale (to center and size the attractor w.r.t 
			// to the local position of the attractor)
			Vector3 phyPos = (Evolve(DT)  + chaosEqn.paramBundle.offset) * chaosEqn.paramBundle.scale;
			// check for blowup
			if (float.IsNaN(phyPos.x) || float.IsNaN(phyPos.y) || float.IsNaN(phyPos.z)) {
				Debug.LogWarning("Position diverged");
				diverged = true;
				return;
			}

			// apply world offset and world scaling
			position = Vector3.Scale(phyPos, evolveScale) + initialPosition;
			position = transform.rotation * position;
			if (chaosTrail != null) {
				if (Vector3.Distance(lastTrail, position) > chaosTrail.minVertexDistance) {
					chaosTrail.AddPoint(position);
					lastTrail = position;
				}
			}
		}
		transform.position = position;
	}

	public ChaosEqn GetEquation() 
	{
		return chaosEqn;
	}
}
