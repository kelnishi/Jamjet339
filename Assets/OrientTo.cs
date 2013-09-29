using UnityEngine;
using System.Collections;

public class OrientTo : MonoBehaviour {
	
	public Vector3 euler = new Vector3(0f,0f,0f);
	
	public Transform position;
	
	public Vector3 offset;
	
	void Start() {
		offset = transform.position - position.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (position) {
			transform.position = position.position + offset;	
		}
		transform.rotation = Quaternion.Euler(euler);
	}
}
