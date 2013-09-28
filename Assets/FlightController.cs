using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class FlightController : MonoBehaviour {
	
	public Vector3 lift = Vector3.up;
	public Vector3 thrust = Vector3.zero;
	public Rigidbody liftBody;
	public Rigidbody thrustBody;
	
	
	void Start () {
		
	}
	
	void Update () {
		if (liftBody) {
			liftBody.AddForce(lift/(rigidbody.mass*liftBody.mass));	
		}
		if (thrustBody) {
			thrustBody.AddRelativeForce(thrust);
		}
	}
}
