using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class planeController : MonoBehaviour {

	private Rigidbody biplane;

	public float accelerationRate = 1.0f;
	public float brakeRate = 0.5f;
	public float forwardSpeedMin = -20.0f;
	public float forwardSpeedMax = 30.0f;

	public float rollRate = 0.55f;
	public float rollMin = -10.0f;
	public float rollMax = 10.0f;
	public float rollFadeRate = 0.5f;

	public float pitchRate = 0.55f;
	public float pitchMin = -10.0f;
	public float pitchMax = 10.0f;
	public float pitchFadeRate = 0.5f;

	/* Current values */
	private Vector3 currentVelocity = Vector3.zero;
	private Quaternion currentRotation = Quaternion.identity;
	private float forwardSpeed;
	private float roll;
	private float pitch;

	/* UI */
	public Text txtVelocity;
	public Text txtRoll;
	public Text txtPitch;
	public Text txtAltitude;

	void Start () {
		forwardSpeed = 0.0f;
		roll = 0.0f;
		pitch = 0.0f;
		biplane = gameObject.GetComponent<Rigidbody>();

		updateUI (forwardSpeed, roll, pitch, biplane.position.y );
	}
	
	// Update is called once per frame
	void Update () {

		/* THROTTLE */
		if(Input.GetKey(KeyCode.W) ){
			forwardSpeed = increase( forwardSpeed, accelerationRate);
		}
		if(Input.GetKey(KeyCode.S) ){
			forwardSpeed = increase( forwardSpeed, -accelerationRate);
		}

		/* PITCH */
		if (Input.GetKey (KeyCode.DownArrow)) {
			//biplane.AddTorque ( transform.InverseTransformDirection(Vector3.left*verticalSensivity) );
			pitch = increase(pitch, -pitchRate);
		}
		if (Input.GetKey (KeyCode.UpArrow)) {
			//biplane.AddTorque ( transform.InverseTransformDirection( Vector3.right*verticalSensivity ) );
			pitch = increase(pitch, pitchRate);
		}

		/* ROLL */
		if (Input.GetKey (KeyCode.LeftArrow)) {
			roll = increase(roll, rollRate);
		}

		if (Input.GetKey (KeyCode.RightArrow)) {
			roll = increase(roll, -rollRate);
		}

		/*if (Input.GetKeyDown(KeyCode.Mouse0)) {
			//RaycastHit hit;
			//Ray ray = Camera.main.ScreenPointToRay(new Vector3(200,200,0.0f ));
			//Ray ray = new Ray (biplane.position, transform.TransformDirection(Vector3.forward*10) );
			//bool isHit = Physics.Raycast (ray, out hit, 1000);
			GameObject lineObject = new GameObject ();
			LineRenderer line = lineObject.AddComponent<LineRenderer> ();
			line.SetPosition (0, ray.origin);
			line.SetPosition (1, ray.direction);
		}*/
		
		/*применение параметров крена*/
		roll = Mathf.Clamp (roll, rollMin, rollMax);

		/*применение параметров тангажа*/
		pitch = Mathf.Clamp (pitch, pitchMin, pitchMax);

		currentRotation = ( Quaternion.Euler( 0.0f, 0.0f, roll) * Quaternion.Euler(pitch, 0.0f, 0.0f) ) ;
		biplane.rotation = biplane.rotation * currentRotation;

		/*применение параметров скорости*/
		forwardSpeed = Mathf.Clamp (forwardSpeed, forwardSpeedMin, forwardSpeedMax);
		currentVelocity = transform.TransformDirection(Vector3.forward)* forwardSpeed;
		biplane.velocity = currentVelocity;

		updateUI (forwardSpeed, roll, pitch, biplane.position.y);

		/*затухание*/
		roll = fade (roll, rollFadeRate);
		pitch = fade (pitch, pitchFadeRate);
	}

	void OnCollisionEnter(){
		/*Vector3 oppositeSpeed = new Vector3 (-biplane.velocity.x, -biplane.velocity.y, -biplane.velocity.z);
		biplane.velocity = oppositeSpeed;

		Quaternion oppositeRotation = new Quaternion (-biplane.rotation.x, -biplane.rotation.y, biplane.rotation.z, biplane.rotation.w);
		biplane.rotation = oppositeRotation;
		*/
	}

	/*Добавляет затухания к величине val со скоростью speed*/
	private float fade(float val, float fadeSpeed){
		if (val > -fadeSpeed && val < fadeSpeed) {
			return 0.0f;
		} else {
			return Mathf.Sign (val) * (Mathf.Abs (val) - fadeSpeed);
		}
	}

	/**/
	private float increase(float val, float sensivity){
		return (val + sensivity);
	}

	/**/
	private void updateUI(float vel, float bank, float pitch, float alt){
		txtVelocity.text = "Velocity: " + vel.ToString ();
		txtRoll.text = "Roll: " + roll.ToString ();
		txtPitch.text = "Pitch: " + pitch.ToString ();
		txtAltitude.text = "Alt: " + alt;
 	}

	/*When the plain get destroyed we need to reset all its parameters to default*/
	public void setDefaultValues() {
		biplane.position = new Vector3 (0.0f, 2.38f, -31.55f);
		currentVelocity = Vector3.zero;
		currentRotation = Quaternion.identity;
		forwardSpeed = 0.0f;
		roll = 0.0f;
		pitch = 0.0f;
	}
}
