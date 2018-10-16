using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class planeController : MonoBehaviour {

	private Rigidbody biplane;
	public float accelerationRate; //1
	public float brakeRate;        //0.5
	public float forwardSpeedMin;  //-20
	public float forwardSpeedMax;  //20

	public float bankRate;         // 
	public float bankMin;
	public float bankMax;
	public float bankFadeRate;

	public float pitchRate;
	public float pitchMin;
	public float pitchMax;
	public float pitchFadeRate;

	private Vector3 currentVelocity = Vector3.zero;
	private Quaternion currentRotation = Quaternion.identity;
	private float forwardSpeed;
	private float bank;
	private float pitch;

	//UI

	public Text txtVelocity;
	public Text txtBank;
	public Text txtPitch;
	public Text txtAltitude;

	void Start () {
		forwardSpeed = 0.0f;
		bank = 0.0f;
		pitch = 0.0f;
		biplane = gameObject.GetComponent<Rigidbody>();

		updateUI (forwardSpeed, bank, pitch, biplane.position.y );
	}
	
	// Update is called once per frame
	void Update () {

		/*ТЯГА*/
		if(Input.GetKey(KeyCode.W) ){
			forwardSpeed = increase( forwardSpeed, accelerationRate);
		}

		if(Input.GetKey(KeyCode.S) ){
			forwardSpeed = increase( forwardSpeed, -accelerationRate);
		}

		/*ТАНГАЖ*/
		if (Input.GetKey (KeyCode.DownArrow)) {
			//biplane.AddTorque ( transform.InverseTransformDirection(Vector3.left*verticalSensivity) );
			pitch = increase(pitch, -pitchRate);
		}

		if (Input.GetKey (KeyCode.UpArrow)) {
			//biplane.AddTorque ( transform.InverseTransformDirection( Vector3.right*verticalSensivity ) );
			pitch = increase(pitch, pitchRate);
		}

		/*КРЕН*/
		if (Input.GetKey (KeyCode.LeftArrow)) {
			bank = increase(bank, bankRate);
		}

		if (Input.GetKey (KeyCode.RightArrow)) {
			bank = increase(bank, -bankRate);
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
		bank = Mathf.Clamp (bank, bankMin, bankMax);

		/*применение параметров тангажа*/
		pitch = Mathf.Clamp (pitch, pitchMin, pitchMax);

		currentRotation = ( Quaternion.Euler( 0.0f, 0.0f, bank) * Quaternion.Euler(pitch, 0.0f, 0.0f) ) ;
		biplane.rotation = biplane.rotation * currentRotation;

		/*применение параметров скорости*/
		forwardSpeed = Mathf.Clamp (forwardSpeed, forwardSpeedMin, forwardSpeedMax);
		currentVelocity = transform.TransformDirection(Vector3.forward)* forwardSpeed;
		biplane.velocity = currentVelocity;

		updateUI (forwardSpeed, bank, pitch, biplane.position.y);

		/*затухание*/
		bank = fade (bank, bankFadeRate);
		pitch = fade (pitch, pitchFadeRate);
	}

	void OnCollisionEnter(){
		Vector3 oppositeSpeed = new Vector3 (-biplane.velocity.x, -biplane.velocity.y, -biplane.velocity.z);
		biplane.velocity = oppositeSpeed;

		Quaternion oppositeRotation = new Quaternion (-biplane.rotation.x, -biplane.rotation.y, biplane.rotation.z, biplane.rotation.w);
		biplane.rotation = oppositeRotation;
		
	}

	/*Добавляет затухание к величине val со скоростью speed*/
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
		txtBank.text = "Bank: " + bank.ToString ();
		txtPitch.text = "Pitch: " + pitch.ToString ();
		txtAltitude.text = "Alt: " + alt;
 	}
}
