using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlaneCurrentValues : MonoBehaviour
{
	//current parameters
	protected float forwardSpeed;
	protected float roll;
	protected float pitch;

	public void resetPlaneCurrentValues(){
		forwardSpeed = 0.0f;
		roll = 0.0f;
		pitch = 0.0f;
	}
}
public class planeController : PlaneCurrentValues {

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
	private Vector3 defaultPosition;

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
		defaultPosition = biplane.position;

		updateUI (forwardSpeed, roll, pitch, biplane.position.y );
	}
	
	/* Ну короче логика тут такая, что мы увеличиваем модули трех величин в моменты, когда соответствующие кнопки
	зажаты. Величины эти: скорость, крен (вращение вокруг продольной оси) и тангаж (вращение вокруг поперечной оси).
	Рыскание пока не учитывается. Первый блок кода в функции отвечает за отслеживание кнопок и увеличение параметров.
	Здесь же параметры ограничиваются по росту ( 6 величин min и max, которые задаются из инспектора)
	и там же идет применение полученых параметров к самолету. В конце функции находится третий блок, который организует
	затухание двух параметров, отвечающих за вращение. Это сделано для того, чтобы исключить бесконечное вращение. То
	есть, каждый раз чтобы повернуть нужно нажимать кнопки поворота. Обратный случай со скоростью - она не затухающая.
	Это значит что можно подержать кнопку тяги какое-то время и после ее отпустить и скорость самолета останется и не
	будет усменьшаться. В оригинальной игре было сделано аналогично - газ держать постоянно не нужно.
	 */
	 /* TODO
	 * 
	 * 
	  */
	void Update () {
		#region block 1 increasing values
			/* THROTTLE */
			if(Input.GetKey(KeyCode.W) && forwardSpeed < forwardSpeedMax ){
				forwardSpeed = increase( forwardSpeed, accelerationRate);
			}
			if(Input.GetKey(KeyCode.S) && forwardSpeed > forwardSpeedMin ){
				forwardSpeed = increase( forwardSpeed, -accelerationRate);
			}

			/* PITCH */
			if (Input.GetKey (KeyCode.DownArrow) && pitch > pitchMin ) {
				//biplane.AddTorque ( transform.InverseTransformDirection(Vector3.left*verticalSensivity) );
				pitch = increase(pitch, -pitchRate);
			}
			if (Input.GetKey (KeyCode.UpArrow) && pitch < pitchMax ) {
				//biplane.AddTorque ( transform.InverseTransformDirection( Vector3.right*verticalSensivity ) );
				pitch = increase(pitch, pitchRate);
			}

			/* ROLL */
			if (Input.GetKey (KeyCode.LeftArrow) && roll < rollMax) {
				roll = increase(roll, rollRate);
			}

			if (Input.GetKey (KeyCode.RightArrow) && roll > rollMin) {
				roll = increase(roll, -rollRate);
			}
		#endregion

			if(Input.GetKey( KeyCode.H)) {
				//biplane.rotation = Quaternion.identity;
				biplane.angularVelocity = Vector3.zero;
			}
		#region block 2 applying values
			/* Вычисление финального кватерниона для применения суммарного вращательного движения к самолету*/
			biplane.angularVelocity = biplane.transform.right * pitch + biplane.transform.forward * roll;

			/* Применение параметров скорости */
			biplane.velocity = biplane.transform.forward * forwardSpeed;
		#endregion

		updateUI (forwardSpeed, roll, pitch, biplane.position.y);

		#region fading of values
			roll = fade (roll, rollFadeRate);
			pitch = fade (pitch, pitchFadeRate);
		#endregion
		if( Input.GetKey( KeyCode.R )) {
			setDefaultValues();
		}
	}

	void OnCollisionEnter(){
		/*Vector3 oppositeSpeed = new Vector3 (-biplane.velocity.x, -biplane.velocity.y, -biplane.velocity.z);
		biplane.velocity = oppositeSpeed;

		Quaternion oppositeRotation = new Quaternion (-biplane.rotation.x, -biplane.rotation.y, biplane.rotation.z, biplane.rotation.w);
		biplane.rotation = oppositeRotation;
		*/
	}

	/* Добавляет затухания к величине val со скоростью speed.
	if - если переданное значение уже меньше коэффициента затухания, то просто вернуть ноль - то есть
	затухнуть полностью. Это сделано чтобы переданное значение не изменило знак.
	else - знак и модуль значения разделяются, чтобы увеличить только модуль НЕ МЕНЯЯ знака.
	*/
	private float fade(float val, float fadeSpeed){
		if (val > -fadeSpeed && val < fadeSpeed) {
			return 0.0f;
		} else {
			return Mathf.Sign (val) * (Mathf.Abs (val) - fadeSpeed);
		}
	}

	/* Ваще элементарно по-моему - эта функция позволяет наращивать разные величины с их индивидуальной
	скоростью.
	*/
	private float increase(float val, float sensivity){
		return (val + sensivity);
	}

	/* Обновляет графический интерфейс. Надо бы передавать сюда массив
	*/
	private void updateUI(float vel, float bank, float pitch, float alt){
		txtVelocity.text = "Velocity: " + vel.ToString ();
		txtRoll.text = "Roll: " + roll.ToString ();
		txtPitch.text = "Pitch: " + pitch.ToString ();
		txtAltitude.text = "Alt: " + alt;
 	}

	/*When the plain get destroyed we need to reset all its parameters to default*/
	public void setDefaultValues() {
		biplane.position = defaultPosition;
		biplane.rotation = Quaternion.identity;
		biplane.angularVelocity = Vector3.zero;
		biplane.velocity = Vector3.zero;
		resetPlaneCurrentValues();
	}
}
