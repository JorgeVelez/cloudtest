using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bound stats.
/// Simple method to track min/max in each co-ordinate to determine the extent
/// of an attractor. Logs the result to the console. 
///
/// Used to measure, then tune the offset and global scale for
/// a parameter bundle. It computes the global scale and offset to 
/// contain the system within a bounding box of <desired_size>.
/// </summary>
public class BoundStats : MonoBehaviour {

	public float log_interval = 15;
	//! Size of the bounding box into which system should fit
	public float desired_size = 10; 
	private float[] minP; 
	private float[] maxP;

	private float last_log;

	// Use this for initialization
	void Start () {
		Reset();
		last_log = Time.time;
	}
	
	private void Reset () {
		minP = new float[]{ float.MaxValue, float.MaxValue, float.MaxValue};
		maxP = new float[]{ float.MinValue, float.MinValue, float.MinValue};
		last_log = Time.time;
	}

	// Update is called once per frame
	void Update () {
		Vector3 p = transform.position;
		minP[0] = Mathf.Min( minP[0], p.x);
		minP[1] = Mathf.Min( minP[1], p.y);
		minP[2] = Mathf.Min( minP[2], p.z);
		maxP[0] = Mathf.Max( maxP[0], p.x);
		maxP[1] = Mathf.Max( maxP[1], p.y);
		maxP[2] = Mathf.Max( maxP[2], p.z);

		if ((Time.time - last_log) > log_interval) {
			last_log = Time.time;
			Debug.Log(string.Format(" x=({0:000.0},{1:000.0}) y=({2:000.0},{3:000.0}) z=({4:000.0},{5:000.0})", 
					minP[0], maxP[0],
					minP[1], maxP[1],
					minP[2], maxP[2]
					));
			// Determine the offset and global scale for a canonical form
			float midX = (maxP[0] + minP[0])/2f;
			float midY = (maxP[1] + minP[1])/2f;
			float midZ = (maxP[2] + minP[2])/2f;
			float maxExtent = Mathf.Max((maxP[0] - minP[0]), (maxP[1] - minP[1]));
			maxExtent = Mathf.Max(maxExtent, (maxP[2] - minP[2]));
			// desired_Scale = maxEtent * scale, so
			float scale = desired_size/maxExtent;
			Debug.Log(string.Format("offset/scale = Vector3({0:00.0}f,{1:00.0}f,{2:00.0}f), {3:0.00}f", 
					-midX, -midY, -midZ, scale
					));
			// this allows early initial position to get discarded. 
			Reset();
		}

	}
}
