using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
/// <summary>
/// Bounding box.
/// Draw a box centered at the transform position using a line renderer. Used to give a 
/// spatial reference when tuning the global scale of a chaotic system.
/// </summary>
public class BoundingBox : MonoBehaviour {

	//! Size of the edge of the box
	public float size = 10.0f;

	private LineRenderer lineRenderer; 

	// Use this for initialization
	void Awake () {
		lineRenderer = GetComponent<LineRenderer>();

		// use cube
		Vector3[] corners = { 
			new Vector3(0.5f,0.5f,0.5f), 
			new Vector3(0.5f,-0.5f,0.5f), 
			new Vector3(-0.5f,-0.5f,0.5f), 
			new Vector3(-0.5f,0.5f,0.5f), 
			new Vector3(0.5f,0.5f,0.5f), 	// end of top face
			new Vector3(0.5f,0.5f,-0.5f),   // down to bottom face
			new Vector3(0.5f,-0.5f,-0.5f), 
			new Vector3(-0.5f,-0.5f,-0.5f), 
			new Vector3(-0.5f,0.5f,-0.5f), 
			new Vector3(0.5f,0.5f,-0.5f), 	// end of bottom face
			new Vector3(0.5f,-0.5f,-0.5f),  // over to next corner
			new Vector3(0.5f,-0.5f,0.5f),   // up 
			new Vector3(-0.5f,-0.5f,0.5f),  // over
			new Vector3(-0.5f,-0.5f,-0.5f),  // down
			new Vector3(-0.5f,0.5f,-0.5f), 	// over
			new Vector3(-0.5f,0.5f,0.5f), 	// up

		};
		lineRenderer.positionCount = corners.Length;

		for (int i=0; i < corners.Length; i++) {
			corners[i] *= size;
		}
		lineRenderer.SetPositions(corners);

	}
	

}
