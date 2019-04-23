using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ChaoticMotion))]
public class Slideshow : MonoBehaviour {

	//! Time interval to show each system for
	public int timer = 15; 

	public GameObject cameraBoom;
	public float spinRate = 1f;

	public Text slideLabel;

	private float nextSlideTime; 

	private int currentSlide = 0; 
	private int maxSlide; 
	private List<ChaosEqn> eqns;
	private string[] eqnNames;

	private ChaoticMotion chaoticMotion;
	private ChaosTrail chaosTrail; 

	private Quaternion initialRotation; 

	// Use this for initialization
	void Start () {
		nextSlideTime = Time.time + timer;
		eqns = ChaosEqnFactory.GetEquations();
		maxSlide = eqns.Count;
		eqnNames = new string[maxSlide];
		int index = 0; 
		foreach (ChaosEqn eqn in eqns) {
			eqnNames[index] = eqn.GetName();
			index++;
		}
		chaoticMotion = GetComponent<ChaoticMotion>();
		chaoticMotion.selectedEqn = currentSlide;
		chaoticMotion.timeZoom = eqns[currentSlide].GetSlideshowSpeed();
		slideLabel.text = eqnNames[currentSlide];
		chaoticMotion.Init();
		// get trail (if present)
		chaosTrail = GetComponentInChildren<ChaosTrail>();

		initialRotation = transform.rotation;

	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > nextSlideTime) {
			nextSlideTime = Time.time + timer;
			NextSlide(true);

		} else {
			if (Time.time < (nextSlideTime - timer/2f)) {
				cameraBoom.transform.rotation *= Quaternion.AngleAxis( spinRate, Vector3.up);
			} else {
				cameraBoom.transform.rotation *= Quaternion.AngleAxis( spinRate, Vector3.right);
			}

		}
		// press "N" for next slide, "P" for previous
		if (Input.GetKeyUp(KeyCode.N)) {
			NextSlide(true);
		} else if (Input.GetKeyUp(KeyCode.P)) {
			NextSlide(false);
		}

	}

	private void NextSlide(bool advance) {
		if (advance) {
			currentSlide++;
			if (currentSlide >= maxSlide) {
				currentSlide = 0; 
			}
		} else {
			currentSlide--;
			if (currentSlide < 0) {
				currentSlide = maxSlide-1; 
			}
		}
		Debug.Log(eqnNames[currentSlide]);
		chaoticMotion.selectedEqn = currentSlide;
		slideLabel.text = eqnNames[currentSlide];
		// re-center position/camera
		transform.position = Vector3.zero;
		cameraBoom.transform.rotation = initialRotation;
		chaoticMotion.timeZoom = eqns[currentSlide].GetSlideshowSpeed();
		chaoticMotion.Init();
		if (chaosTrail != null) {
			chaosTrail.Init();
		}

	}
}
