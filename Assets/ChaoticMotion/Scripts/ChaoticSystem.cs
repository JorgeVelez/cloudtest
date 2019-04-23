using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*! \mainpage Chaotic Motion Unity Asset
 *
 * Chaotic motion provides an extensive collection of 3D chaos equations that can be used to 
 * evolve game object or particle motion so they move chaotically. 
 *
 * ChaoticSystem is the common base class for motion of objects and particles. It is extended by:
 *
 *   ChaoticMotion - movement for game objects
 *   ChaoticParticles - movement for a particle system
 *
 * If you are interested in adding additional systems - this can be easily done by cloning one of the
 * systems, modifying the code and then adding your new system to the list in ChaosEqnFactory. 
 * 
 * On-line documentation: <a href="http://nbodyphysics.com/blog/chaotic-motion/">Online</a>
 *
 */

/// <summary>
/// Chaotic system.
/// Base class for chaotic scripts. Holds the type of system selected, 
/// the parameter set or the custom parameters to be used. 
/// </summary>
public class ChaoticSystem : MonoBehaviour {

	public const string NO_PARAM = "none";

	//! Time scale (>0)
	public float timeZoom = 1f;

	//! Number of chaos system to evolve (w.r.t. ChaosFactory list)
	public int selectedEqn;
	//! Number of parameter bundle selected for evolution (per chaotic equation)
	public int selectedParams;

	//! Scale to apply to phyics evolution when mapping back to world space
	public Vector3 evolveScale = Vector3.one;

	//! Custom parameter values for evolution (valid only if selectedParams exceeds number of params listed in chaosEqn)
	public ParamBundle customParams = null; 

	//! Editor foldout tab status
	public bool paramFoldout = false;
	public bool speedFoldout = false;

	public virtual void Init() {
		Debug.LogError("Must override this method");
	}

	// Time evolution 
	// time evolution
	protected float physicsTime; 
	protected float worldTime; 
	// minimum DT is driven by evolution of Lorenz on outer loops (where it moves fast)
	protected float DT = 0.005f;

	protected void TimeInit() {
		worldTime = Time.time;
		physicsTime = worldTime*timeZoom;
	}

	protected int CalcNumSteps() {

		worldTime += Time.fixedDeltaTime;
		float timeSlice = Mathf.Max(worldTime - physicsTime/timeZoom, 0); 
		int numSteps = Mathf.RoundToInt(timeSlice/DT);
		physicsTime += numSteps * DT;
		return numSteps; 
	}

}
