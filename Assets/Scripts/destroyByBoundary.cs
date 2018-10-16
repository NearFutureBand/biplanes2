using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyByBoundary : MonoBehaviour {

	void OnTriggerExit(Collider other){
		if ( !other.gameObject.CompareTag("Player")) {
			print(other.gameObject.tag);
			Destroy (other.gameObject);
		} 

	}
}
