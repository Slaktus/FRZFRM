using UnityEngine;
using System.Collections;

public class AvatarMovementController : MonoBehaviour {
	
	private Rigidbody thisRigidbody;
	private Transform thisTransform;
	
	void Awake () {
		thisRigidbody = rigidbody;
		thisTransform = transform;
	}
	
	private Transform cursorTransform;
	
	void Start () {
		cursorTransform = GameObject.FindGameObjectWithTag( "Cursor" ).transform;
	}
	
	public float movementForce;
	
	private void MoveToPoint ( Vector3 targetPosition ) {
		thisRigidbody.AddForce( Vector3.Normalize(  targetPosition - thisTransform.position ) * movementForce , ForceMode.Acceleration );
	}
	
	public float minDistanceToCursor;
	public float inputDrag;
	public float noInputDrag;
	
	// Update is called once per frame
	void Update () {
		if ( Input.GetMouseButton( 0 ) ) {
			if ( thisRigidbody.drag > inputDrag ) thisRigidbody.drag = inputDrag;
			if ( Vector3.Distance( thisTransform.position , cursorTransform.position ) > minDistanceToCursor )  MoveToPoint( cursorTransform.position );
		} else if ( thisRigidbody.drag < noInputDrag ) thisRigidbody.drag = noInputDrag;
		thisRigidbody.rotation = Quaternion.LookRotation( thisRigidbody.velocity.normalized );
	}
}
