using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour {

	//Цель
	public Transform target;

	//Скорость слежения
	public float speed = 4f;

	//private Vector3 currentPosition;
	//private Quaternion tmpQuaternion;

	//Маска слоев препятствий
	//public LayerMask maskObstacles;

	private Vector3 _position;

	void Start () {
		_position = target.InverseTransformPoint(transform.position);
	}
	void FixedUpdate ()
	{
		var oldRotation = target.rotation;
		target.rotation = Quaternion.Euler(0, oldRotation.eulerAngles.y, 0);
		var currentPosition = target.TransformPoint(_position);
		target.rotation = oldRotation;

		transform.position = Vector3.Lerp(transform.position, currentPosition, speed * Time.deltaTime);
		var currentRotation = Quaternion.LookRotation(target.position - transform.position);
		transform.rotation = Quaternion.Lerp(transform.rotation, currentRotation, speed * Time.deltaTime);
	}
}
