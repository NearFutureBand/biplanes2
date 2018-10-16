using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : MonoBehaviour {

	public GameObject shot;
	private Transform machineGunTransform;
	public float fireRate;
	private float nextFire;
	// Use this for initialization
	void Start () {
		machineGunTransform = GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton ("Fire1") && Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			Instantiate (shot, machineGunTransform.position, machineGunTransform.rotation);
		}
	}
}
