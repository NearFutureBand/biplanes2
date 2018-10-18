using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyByGround : MonoBehaviour {

	public Rigidbody biplane;

	void Start () {
		
	}

	void OnTriggerEnter(Collider other){
		//if ( !other.gameObject.CompareTag("Player")) {
		//print(other.gameObject.tag);
		print(other.gameObject.name);
		//Destroy (plane);


		biplane.position = new Vector3 (0.0f, 2.38f, -31.55f);
		biplane.rotation = Quaternion.identity;
		biplane.velocity = Vector3.zero;


		//plane.setDefaultValues();

		//Instantiate(plane.transform, new Vector3(0.0f, 2.38f, -31.55f), Quaternion.identity);

		//Destroy (other.gameObject);
		//}
	}
}
