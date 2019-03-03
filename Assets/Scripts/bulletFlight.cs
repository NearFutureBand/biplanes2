using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletFlight : MonoBehaviour {

	public float bulletSpeed;
	public float selfDestroyTime = 10.0f;
	private Rigidbody bullet;
	// Use this for initialization
	void Start () {
		bullet = GetComponent<Rigidbody> ();
		bullet.velocity = transform.forward * bulletSpeed;
		Destroy( gameObject, selfDestroyTime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
