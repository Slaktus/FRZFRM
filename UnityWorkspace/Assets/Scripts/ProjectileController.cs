using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour {
	
	private Transform thisTransform;
	private Rigidbody thisRigidbody;
	
	void Awake () {
		thisTransform = transform;
		thisRigidbody = rigidbody;
	}
	
	public float impactImpulseStrength;
	
	private Vector3 impactDirection;
	private Transform hitObjectTransform;
	
	void OnTriggerEnter ( Collider collider ) {
		if ( isHoming && collider.gameObject == targetEnemy ) {
			impactDirection = Vector3.Normalize( collider.transform.position - thisTransform.position );
			collider.gameObject.rigidbody.AddForceAtPosition( impactDirection * impactImpulseStrength , thisTransform.position , ForceMode.Impulse );
			collider.gameObject.SendMessage( "ProjectileHit" , SendMessageOptions.DontRequireReceiver );
			CleanUpProjectile();
		}
	}
	
	public GameObject hitParticles;
	
	private GameObject hitEffect;
	
	public void CleanUpProjectile () {
		thisTransform.BroadcastMessage( "DetachTrail" );
		hitEffect = Instantiate( hitParticles , thisTransform.position , Quaternion.identity ) as GameObject;
		Destroy( gameObject );
	}
	
	public float ejectionDistance;
	public float ejectionDuration;
	public Vector3 ejectionDirection;
	public GameObject targetEnemy;
	
	private Transform targetTransform;
	private bool isHoming;
	private Vector3 initialPosition;
	
	void Start () {
		initialPosition = thisTransform.position;
		targetTransform = targetEnemy.transform;
		Go.to( thisTransform , ejectionDuration , new TweenConfig().position( initialPosition + ( ejectionDirection * ejectionDistance ) ).setEaseType( EaseType.BackOut ) ).setOnCompleteHandler( home => isHoming = true );
		homingDirection = ejectionDirection;
	}
	
	public float homingSpeed;
	public bool useSlerp;
	public float turningSpeed;
	public float homingSpeedDecayRate;
	public float turningSpeedIncreaseRate;
	
	private Vector3 targetPosition;
	private float distanceFromTarget;
	private Vector3 targetDirection;
	private Vector3 homingDirection;
	
	// Update is called once per frame
	void Update () {
		if ( isHoming ) {
			if ( targetTransform == null ) CleanUpProjectile();
			else if ( useSlerp ) thisTransform.position = Vector3.Slerp( thisTransform.position , targetTransform.position , Time.deltaTime * homingSpeed );
			else {
				targetDirection = targetTransform.position - thisTransform.position;
				homingDirection = Vector2.Lerp( homingDirection , targetDirection , Time.deltaTime * turningSpeed );
				homingDirection.Normalize();
				thisTransform.position += ( Vector3 ) homingDirection * ( ( Time.deltaTime * homingSpeed ) );
				thisTransform.rotation = Quaternion.LookRotation( homingDirection );
				homingSpeed *= homingSpeedDecayRate;
				turningSpeed *= turningSpeedIncreaseRate;
			}
			if ( targetTransform != null ) thisTransform.rotation = Quaternion.LookRotation( targetTransform.position - thisTransform.position , Vector3.forward );
		} else thisTransform.rotation = Quaternion.LookRotation( ejectionDirection , Vector3.forward );
	}
}
