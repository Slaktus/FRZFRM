using UnityEngine;
using System.Collections;

public class EnemyMovementController : MonoBehaviour {
	
	private Rigidbody thisRigidbody;
	private Transform thisTransform;
	
	void Awake () {
		thisRigidbody = rigidbody;
		thisTransform = transform;
	}
	
	private Transform avatarTransform;
	
	// Use this for initialization
	void Start () {
		avatarTransform = GameObject.FindGameObjectWithTag( "Avatar" ).transform;
	}
	
	public float movementForce;
	
	private void MoveToPoint ( Vector3 targetPosition ) {
		thisRigidbody.AddForce( Vector3.Normalize(  targetPosition - thisTransform.position ) * movementForce , ForceMode.Acceleration );
	}
	
	// Update is called once per frame
	void Update () {
		MoveToPoint( avatarTransform.position );
	}
}
