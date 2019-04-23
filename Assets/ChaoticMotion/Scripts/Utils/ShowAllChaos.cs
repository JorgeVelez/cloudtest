using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Sample code to invoke a Chaos prefab for all equations provided by the factory
/// and locate them in an N x N grid in X and Y. The prefab may contain multiple chaoticMotion
/// or ChaosParticles objects.
///
/// Used for the overview scene. 
///
/// </summary>
public class ShowAllChaos : MonoBehaviour {

	public GameObject chaosPrefab; 
	public float gridSpacing = 30f; 

	public Material[] materials;

	// Use this for initialization
	void Start () {
		int count = ChaosEqnFactory.GetEquations().Count;
		int gridSize = Mathf.CeilToInt(Mathf.Sqrt((float) count));
		int row = 0;
		int col = 0;
		int materialCount = 0; 
		// set position in grid
		if (materials.Length == 0 ) {
			Debug.Log("Need to specify materials");
		}
		Vector3 origin = new Vector3(-gridSpacing*gridSize/2, gridSpacing*gridSize/2, 0f);
		for (int i=0; i < count; i++) {
			Vector3 p = transform.position + origin; 
			p.x += gridSpacing * row;
			p.y -= gridSpacing * col;			

			GameObject chaosSystem = Instantiate<GameObject>(chaosPrefab, p, Quaternion.identity);
			chaosSystem.transform.parent = transform;
			ChaoticMotion cm = chaosSystem.GetComponentInChildren<ChaoticMotion>();
			cm.selectedEqn = i;
			cm.timeZoom = cm.GetEquation().GetSlideshowSpeed();
			cm.Init();
			// Use the specified materials to color the attractor trails differently
			foreach ( LineRenderer lr in cm.GetComponentsInChildren<LineRenderer>()) {
				lr.sharedMaterial = materials[materialCount];
			}
			Text t = chaosSystem.GetComponentInChildren<Text>();
			t.text = cm.GetEquation().GetName();
			// advance to next grid slot
			row++; 
			if (row >= gridSize) {
				row = 0; 
				col++;
			}
			materialCount++;
			if (materialCount >= materials.Length)
					materialCount = 0; 
			Debug.Log("mc= " + materialCount);
		}

	}
	
}
