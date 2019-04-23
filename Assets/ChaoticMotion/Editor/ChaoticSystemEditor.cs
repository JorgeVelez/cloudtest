using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ChaoticSystem), true)]	// true - children also show this editor
public class ChaoticSystemEditor : Editor {

	private string[] eqnNames; 
	private List<ChaosEqn> eqns; 

	private bool eqnFoldout = false;

	void OnEnable() {
		ChaoticSystem chaotic = (ChaoticSystem) target;
		eqns = ChaosEqnFactory.GetEquations();
		eqnNames = new string[eqns.Count];
		int index = 0; 
		foreach (ChaosEqn eqn in eqns) {
			eqnNames[index] = eqn.GetName();
			index++;
		}
	}

	private const string STEP_SIZE = "Step Size: ";
	private const string STEPS_PER_FRAME = "Steps Per Frame: ";
	private const string INITIAL_POS = "Initial position (physics space) : ";
	private const string OFFSET = "Offset (physics space) : ";
	private const string SCALE = "Scale (physics space) : " ;


	public override void OnInspectorGUI()
	{
		ChaoticSystem chaotic = (ChaoticSystem) target;
		int lastSelectedEqn = chaotic.selectedEqn;
		int selectedParams = chaotic.selectedParams;
		Vector3 evolveScale;
		float timeZoom = chaotic.timeZoom;

		int selectedEqn = EditorGUILayout.Popup("Chaos System:", lastSelectedEqn, eqnNames);

		eqnFoldout = EditorGUILayout.Foldout( eqnFoldout, "Equations");
		if (eqnFoldout) {
			foreach ( string es in eqns[selectedEqn].GetEquationStrings() ) {
				EditorGUILayout.LabelField(es);
			}
		}

		// show bundle choices + custom
		ParamBundle[] paramBundles = eqns[selectedEqn].GetParamBundles();
		// add one to allow custom to be added at end
		string[] pBundleNames = new string[paramBundles.Length+1];
		for( int i=0; i < paramBundles.Length; i++)
			pBundleNames[i] = paramBundles[i].label;
		pBundleNames[paramBundles.Length] = "Custom";

		if (selectedEqn != lastSelectedEqn) {
			// if the equation changes reset to some sensible params for this equation
			selectedParams = 0; 
			chaotic.customParams = new ParamBundle(paramBundles[0]);
		}

		selectedParams = EditorGUILayout.Popup("Parameter Choice:", selectedParams, pBundleNames);

		string[] pNames = eqns[selectedEqn].GetParamNames();
		float[] eqnParam = new float[pNames.Length];

		chaotic.paramFoldout = EditorGUILayout.Foldout( chaotic.paramFoldout, "Parameters");
		// First time need to instantiate the custom param object
		if (chaotic.customParams == null || chaotic.customParams.eqnParam == null) {
			chaotic.customParams = new ParamBundle(paramBundles[0]);
			chaotic.customParams.Log();
		} 
		// To allow Undo on edited params, need to check for value change and then 
		// update sub
		Vector3 initialPosition = chaotic.customParams.initialPosition;
		Vector3 offset = chaotic.customParams.offset;
		float scale = chaotic.customParams.scale;

		if (chaotic.paramFoldout) {
			// show values
			if (selectedParams >= paramBundles.Length) {
				// allow value changes if Custom
				for (int i=0; i < pNames.Length; i++) {
					if (pNames[i] != ChaoticSystem.NO_PARAM) {
						eqnParam[i] = EditorGUILayout.FloatField(pNames[i], chaotic.customParams.eqnParam[i]);
					}
				}
				initialPosition = EditorGUILayout.Vector3Field(INITIAL_POS, chaotic.customParams.initialPosition);
				offset = EditorGUILayout.Vector3Field(OFFSET, chaotic.customParams.offset);
				scale = EditorGUILayout.FloatField(SCALE, chaotic.customParams.scale);
			} else {
				for (int i=0; i < pNames.Length; i++) {
					if (pNames[i] != ChaoticSystem.NO_PARAM) {
						EditorGUILayout.LabelField(pNames[i] + " : " + paramBundles[selectedParams].eqnParam[i]);
					}
				}
				EditorGUILayout.LabelField(INITIAL_POS + paramBundles[selectedParams].initialPosition);
				EditorGUILayout.LabelField(OFFSET + paramBundles[selectedParams].offset);
				EditorGUILayout.LabelField(SCALE + paramBundles[selectedParams].scale);
			}
		}

		// timeZoom > 0
		timeZoom = Mathf.Max( EditorGUILayout.FloatField( "Time Zoom", chaotic.timeZoom ), 0);

		evolveScale = EditorGUILayout.Vector3Field("Evolution Scale: ", chaotic.evolveScale);


		if (GUI.changed) {
			Undo.RecordObject(chaotic, "Chaotic Motion Change");
			if (chaotic.paramFoldout && selectedParams >= paramBundles.Length) {
				chaotic.customParams.initialPosition = initialPosition;
				for (int i=0; i < pNames.Length; i++) {
					chaotic.customParams.eqnParam[i] = eqnParam[i];
				}
				chaotic.customParams.offset = offset;
				chaotic.customParams.scale = scale;
			}
			chaotic.selectedEqn = selectedEqn;
			chaotic.selectedParams = selectedParams;
			chaotic.evolveScale = evolveScale;
			chaotic.timeZoom = timeZoom;
			EditorUtility.SetDirty(chaotic);
		}

	}
}
