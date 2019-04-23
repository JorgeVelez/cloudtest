using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// simple "struct" to hold param info associated with a ChaoticEqn

[System.Serializable]
/// <summary>
/// Parameter bundle.
/// Container class to hold the values for starting a chaotic system. Holds
/// the parameters for the equations and the initial position and scale values. 
/// </summary>
public class ParamBundle  {

	//! name of the parameter bundle
	public string label; 

	//! parameters used in the equation
	public float[] eqnParam;

	//! offset to center attractor at local origin 
	public Vector3 offset = Vector3.zero; 

	//! scale to ensure that attractor fits in box of size 10
	public float scale = 1f;

	//! Initial position for evolution in physics space
	public Vector3 initialPosition; 

	// Default offset and scale - 3 param version 
	public ParamBundle(string label, float p0, float p1, float p2, 
			Vector3 initialPosition) {

		eqnParam = new float[] { p0, p1, p2};
		this.label = label;
		this.initialPosition = initialPosition;
	}

	// Default offset and scale - float[] param version 
	public ParamBundle(string label, float[] eqnP, 
			Vector3 initialPosition) {

		eqnParam = new float[eqnP.Length];
		for (int i=0; i < eqnParam.Length; i++) { 
			eqnParam[i] = eqnP[i];
		} 
		this.label = label;
		this.initialPosition = initialPosition;
	}

	// provide offset and scale - 3 param version 
	public ParamBundle(string label, float p0, float p1, float p2, 
			Vector3 initialPosition, 
			Vector3 offset, float scale) {

		eqnParam = new float[] { p0, p1, p2};
		this.label = label;
		this.initialPosition = initialPosition;
		this.offset = offset;
		this.scale = scale;
	}

	// provide offset and scale - float[] param version 
	public ParamBundle(string label, float[] eqnP, 
			Vector3 initialPosition, 
			Vector3 offset, float scale) {

		eqnParam = new float[eqnP.Length];
		for (int i=0; i < eqnParam.Length; i++) { 
			eqnParam[i] = eqnP[i];
		} 
		this.label = label;
		this.initialPosition = initialPosition;
		this.offset = offset;
		this.scale = scale;
	}

	// copy constructor used by Editor script
	public ParamBundle(ParamBundle fromPb) {
		eqnParam = new float[fromPb.eqnParam.Length];
		for (int i=0; i < eqnParam.Length; i++) { 
			eqnParam[i] = fromPb.eqnParam[i];
		} 
		this.label = fromPb.label;
		this.initialPosition = fromPb.initialPosition;
		this.offset = fromPb.offset;
		this.scale = fromPb.scale;
	}

	public void Log() {
		Debug.Log(" ip=" + initialPosition 
			+ " eqn[0]=" + eqnParam[0] + " eqn[1]=" + eqnParam[1] + " eqn[2]=" + eqnParam[2] );
	}

}
