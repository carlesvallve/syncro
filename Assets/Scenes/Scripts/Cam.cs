using UnityEngine;
using System.Collections;


public class Cam : MonoBehaviour {

	public float speed = 0.25f;


	void Start () {
		
	}
	
	
	void Update () {
		transform.Translate(0,0, speed);
	}
}
