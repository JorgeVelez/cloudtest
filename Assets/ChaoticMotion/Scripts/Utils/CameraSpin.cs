using UnityEngine;
using System.Collections;

/// <summary>
/// Key control to rotate camera boom using Arrow keys. Scale with +/-.
///
/// Assumes the Main Camara is a child of the object holding this script with a local position offset
/// (the boom length) and oriented to point at this object. Then pressing the keys will spin the camera
/// around the object this script is attached to.
/// </summary>
public class CameraSpin : MonoBehaviour {

	//! Rate of spin (degrees per Update)
	public float spinRate = 1f; 

	private Transform mainCamera; 

	public float boomStep = 2f;

	private const float boomMin = 2f;
	private const float boomMax = 1000f;

	// Use this for initialization
	void Start () {
		// assume first child is the camera
		mainCamera = transform.GetChild(0);
	}
	
	// Update is called once per frame
	void Update () {
		float boomLength = mainCamera.localPosition.z;

		if (Input.GetKey(KeyCode.RightArrow)) {
			transform.rotation *= Quaternion.AngleAxis( spinRate, Vector3.up);
		} else if (Input.GetKey(KeyCode.LeftArrow)) {
			transform.rotation *= Quaternion.AngleAxis( -spinRate, Vector3.up);
		} else if (Input.GetKey(KeyCode.UpArrow)) {
			transform.rotation *= Quaternion.AngleAxis( spinRate, Vector3.right);
		} else if (Input.GetKey(KeyCode.DownArrow)) {
			transform.rotation *= Quaternion.AngleAxis( -spinRate, Vector3.right);
		} else if (Input.GetKey(KeyCode.Equals)) {
			// shorten the boom 
			boomLength -= boomStep; 
			boomLength = Mathf.Max(boomLength, boomMin);
			Vector3 localP = mainCamera.localPosition;
			localP.z = boomLength;
			mainCamera.localPosition = localP;

		} else if (Input.GetKey(KeyCode.Minus)) {
			// lengthen the boom 
			boomLength += boomStep; 
			boomLength = Mathf.Min(boomLength, boomMax);
			Vector3 localP = mainCamera.localPosition;
			localP.z = boomLength;
			mainCamera.localPosition = localP;
		}	
	}
}
