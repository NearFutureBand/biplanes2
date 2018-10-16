using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletFlight : MonoBehaviour {

	public float bulletSpeed;
	private Rigidbody bullet;
	// Use this for initialization
	void Start () {
		bullet = GetComponent<Rigidbody> ();
		bullet.velocity = transform.forward * bulletSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
