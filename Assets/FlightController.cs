using UnityEngine;
using HutongGames.PlayMaker;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class FlightController : MonoBehaviour {
	
	
	public Vector3 thrust = Vector3.zero;
	
	public float speedThreshold = 0.1f;
	public float untwist = 1f;
	public Rigidbody cork;
	public Rigidbody screw;
	
	public Rigidbody thrustBody;
	
	public float steerForce = 100f;
	public float thrustForce = 200f;
	public float liftForce = 20f;
	
	public float speed;
	
	Vector3 lastPosition;
	
	public float distance = 0f;
	
	public PlayMakerFSM fsm;
	
	public TTFText score;
	
	void Start () {
		foreach (PlayMakerFSM f in GetComponents<PlayMakerFSM>()) {
			if (f.FsmName == "Distance") {
				fsm = f;
				break;
			}
		}
		
		lastPosition = transform.position;
	}
	
	public Quaternion steer = Quaternion.identity;
	public bool thrustOn = false;
	
	
	private Vector3 up;
	
	private bool update = true;
	
	void Update () {
		if (!update) return;
		
		speed = rigidbody.velocity.magnitude;
		
		Vector3 force = Vector3.zero;
		Vector3 steering = steer * Vector3.forward;
		Vector3 heading = transform.forward;
		
		up = -Vector3.Cross(transform.forward, Vector3.Cross(transform.forward, Vector3.up));
		cork.AddForce(up * untwist + Vector3.up * liftForce);
		screw.AddForce(-up *untwist);
		
		if (thrustOn) {
			force += steering * steerForce + heading * thrustForce;
		}
		
		thrustBody.AddForce(force, ForceMode.Acceleration);
		
		distance += (transform.position - lastPosition).magnitude;
		lastPosition = transform.position;
		
		score.Text = ""+ (int)distance+"m";
		
		FsmFloat f = fsm.FsmVariables.GetFsmFloat("Distance");
		f.Value = distance;
	}
	
	void Steer(Quaternion look) {
		steer = look;
	}
	
	void SetThrust(bool on) {
		thrustOn = on;
	}
	
	void Die() {
		update = false;	
	}
	
	void OnDrawGizmos() {
		Gizmos.DrawLine(cork.transform.position, cork.transform.position + up);
		Gizmos.DrawLine(screw.transform.position, screw.transform.position - up);
	}
}
