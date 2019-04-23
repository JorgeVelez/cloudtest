using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ChaoticParticles))]
[RequireComponent(typeof(ParticleSystem))]
public class SlideshowParticles : MonoBehaviour {

	//! Time interval to show each system for
	public int timer = 15; 

	public GameObject cameraBoom;
	public float spinRate = 0.2f;

	public Text slideLabel;

	private float nextSlideTime; 

	private int currentSlide = 0; 
	private int maxSlide; 
	private List<ChaosEqn> eqns;
	private string[] eqnNames;

	private ChaoticParticles chaoticParticles;
	private ParticleSystem particleSys;

	private Quaternion initialRotation;

	public float speedTrim = 1.0f;

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
		chaoticParticles = GetComponent<ChaoticParticles>();
		chaoticParticles.selectedEqn = currentSlide;
		slideLabel.text = eqnNames[currentSlide];
		chaoticParticles.timeZoom = eqns[currentSlide].GetSlideshowSpeed()*speedTrim;
		chaoticParticles.Init();
		particleSys = GetComponent<ParticleSystem>();

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
		chaoticParticles.selectedEqn = currentSlide;
		slideLabel.text = eqnNames[currentSlide];
		// restart particle system
		particleSys.Clear();
		particleSys.Play();

		// re-center position/camera
		transform.position = Vector3.zero;
		cameraBoom.transform.rotation = initialRotation;
		chaoticParticles.timeZoom = eqns[currentSlide].GetSlideshowSpeed()*speedTrim;
		chaoticParticles.Init();

	}
}
