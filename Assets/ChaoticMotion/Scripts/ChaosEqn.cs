using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Chaos eqn.
/// Base class for all equations that define a 3D chaotic system. 
/// </summary>
public abstract class ChaosEqn  {

	protected string name;
	protected string[] eqnStrings;
	protected string[] paramNames;
	protected ParamBundle[] paramBundles;

	protected float slideShowSpeed = 1.0f; 

	//! The parameter bundle that is used to evolve the system
	public ParamBundle paramBundle;

	/// <summary>
	/// Gets the parameter bundles defned for the chaos equation. 
	/// </summary>
	/// <returns>The parameter bundles.</returns>
	public ParamBundle[] GetParamBundles() {
		return paramBundles;
	}

	/// <summary>
	/// Gets the name.
	/// </summary>
	/// <returns>The name.</returns>
	public string GetName() {
		return name;
	}

	/// <summary>
	/// Gets the equation strings. Used by the Inspector to show the system definition. 
	/// </summary>
	/// <returns>The equation strings.</returns>
	public string[] GetEquationStrings() {
		return eqnStrings; 
	}

	/// <summary>
	/// Gets the parameter names. Used by the inspector to label the parameters and prompts
	/// (when custom parameter is selected)
	/// </summary>
	/// <returns>The parameter names.</returns>
	public string[] GetParamNames() {
		return paramNames; 
	}

	/// <summary>
	/// Sets the parameter bunlde to be used by the system.
	/// </summary>
	/// <param name="pb">Pb.</param>
	public abstract void SetParams(ParamBundle pb);

	/// <summary>
	/// Evaluate the first order evolution of the attractor, given the current position. 
	/// </summary>
	/// <param name="x_in">Current position.</param>
	/// <param name="x_dot">Return velocity terms in x_dot.</param>
	public abstract void Function(ref float[] x_in, ref float[] x_dot);


	public float GetSlideshowSpeed() {
		return slideShowSpeed;
	}

}
