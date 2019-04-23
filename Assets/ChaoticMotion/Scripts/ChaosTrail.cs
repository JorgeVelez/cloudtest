using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
/// <summary>
/// Chaos trail.
/// Update a trail using a LineRenderer. This allows the path between points to be filled in 
/// using intermediate values from the numerical integration - avoiding a "splined" appearance
/// when the evolution steps per frame are large. 
///
/// The number of points used is defined by the Line Renderer component. 
/// </summary>
public class ChaosTrail : MonoBehaviour {

	//! Distance interval for points to be added to the renderer
	public float minVertexDistance = 0.1f;
	//! Number of points in the trail
	public float maxPoints = 500;

	private LineRenderer lineRenderer; 

	// use an array as a ring buffer to avoid shuffling as points are added. 
	private List<Vector3> points;

	// Use this for initialization
	void Awake () {
		Init();
	}

	public void Init () {
		lineRenderer = GetComponent<LineRenderer>();
		points = new List<Vector3>();
	}

	/// <summary>
	/// Adds a point to the trail.
	/// </summary>
	/// <param name="point">Point.</param>
	public void AddPoint(Vector3 point) {
		if (points.Count >= maxPoints) {
			points.RemoveAt(0);
		}
		points.Add(point);
		// copy the points into the LineRenderer
		lineRenderer.positionCount = points.Count;
		int i = 0; 
		foreach (Vector3 p in points) {
			lineRenderer.SetPosition(i++, p);
		}
	}

}
