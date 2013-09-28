using UnityEngine;
using System.Collections;

public class OrientTo : MonoBehaviour {
	
	public Vector3 euler = new Vector3(0f,0f,0f);
	
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Euler(euler);
	}
}
