using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyByBoundary : MonoBehaviour {

	public Rigidbody biplane;

	void OnTriggerExit(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			biplane.position = new Vector3 (0.0f, 2.38f, -31.55f);
			biplane.rotation = Quaternion.identity;
			biplane.velocity = Vector3.zero;
			

		} else {
			Destroy (other.gameObject);
		}
	}
}
