using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class FlightController : MonoBehaviour {
	
	public Vector3 lift = Vector3.up;
	public Vector3 thrust = Vector3.zero;
	public Rigidbody liftBody;
	public Rigidbody thrustBody;
	
	public float thrustForce = 1000f;
	public float thrustLift = 100f;
	
	void Start () {
		
	}
	
	public Quaternion steer = Quaternion.identity;
	public bool thrustOn = false;
	
	void Update () {
		if (liftBody) {
			liftBody.AddForce(lift/(rigidbody.mass*liftBody.mass));	
		}
		if (thrustBody & thrustOn) {
			Vector3 heading = steer * Vector3.forward;
			
			thrustBody.transform.rotation = Quaternion.identity;
			thrustBody.transform.LookAt(thrustBody.transform.position + heading);
			
			Vector3 thrustVector = thrustBody.transform.TransformDirection(Vector3.forward * thrustForce);
			thrustBody.AddForce(thrustVector + Vector3.up * thrustLift,ForceMode.Acceleration);
		}
	}
	
	void Steer(Quaternion look) {
		steer = look;
	}
	
	void SetThrust(bool on) {
		thrustOn = on;
	}
}
