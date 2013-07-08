using UnityEngine;
using System.Collections;

public class EnemyMovementController : MonoBehaviour {
	
	public float avatarPollInterval;
	
	private Vector3 bufferedAvatarPosition;
	
	IEnumerator PollAvatarPosition () {
		bufferedAvatarPosition = avatarTransform.position;
		yield return new WaitForSeconds( avatarPollInterval );
		StartCoroutine( PollAvatarPosition() );
	}
	
	private Rigidbody thisRigidbody;
	private Transform thisTransform;
	
	void Awake () {
		thisRigidbody = rigidbody;
		thisTransform = transform;
	}
	
	private bool isTimeStopped;
	
	public void StopTime () {
		bufferedVelocity = thisRigidbody.velocity;
		bufferedAngularVelocity = thisRigidbody.angularVelocity;
		isTimeStopped = true;
	}
	
	public GameObject cursorTag;
	public float cursorTagScaleUpDuration;
	public float cursorTagScaleMultiplier = 2f;
	
	private GameObject currentTag;
	private Vector3 tagTargetScale;
	private bool isTagged;
	
	public void AddTag () {
		if ( isTimeStopped && !isTagged ) {
			currentTag = Instantiate( cursorTag , thisTransform.position , Quaternion.identity ) as GameObject;
			currentTag.transform.parent = thisTransform.parent;
			tagTargetScale = thisTransform.localScale * cursorTagScaleMultiplier;
			Go.to( currentTag.transform , cursorTagScaleUpDuration , new TweenConfig().scale( tagTargetScale , false ).setEaseType( EaseType.BackOut ) );
			isTagged = true;
		}
	}
	
	public int maxHits = 3;
	public float destroyTweenDuration;
	
	private int numberOfHits;
	
	public void ProjectileHit () {
		numberOfHits += 1;
		if ( numberOfHits >= maxHits ) {
			Go.to( thisTransform , destroyTweenDuration , new TweenConfig().scale( Vector3.zero , false ).setEaseType( EaseType.BackIn ) ).setOnCompleteHandler( destroy => CleanUpEnemy() );
		}
	}
	
	public float cursorTagScaleDownDuration;
	
	private Vector3 bufferedVelocity;
	private Vector3 bufferedAngularVelocity;
	
	public void StartTime () {
		StartCoroutine( PollAvatarPosition() );
		thisRigidbody.velocity = bufferedVelocity;
		thisRigidbody.angularVelocity = bufferedAngularVelocity;
		isTimeStopped = false;
		if ( currentTag != null ) Go.to( currentTag.transform , cursorTagScaleDownDuration , new TweenConfig().scale( Vector3.zero , false ).setEaseType( EaseType.BackIn ) ).setOnCompleteHandler( destroy => CleanUpTag() );
	}
	
	private void CleanUpTag () {
		isTagged = false;
		Go.killAllTweensWithTarget( currentTag.transform );
		Destroy( currentTag );
	}
	
	public GameObject enemyExplosionEffect;
	
	private GameObject explosionEffect;
	
	private void CleanUpEnemy () {
		if ( currentTag != null ) Go.to( currentTag.transform , cursorTagScaleDownDuration , new TweenConfig().scale( Vector3.zero , false ).setEaseType( EaseType.BackIn ) ).setOnCompleteHandler( destroy => CleanUpTag() );
		explosionEffect = Instantiate( enemyExplosionEffect , thisTransform.position , Quaternion.identity ) as GameObject;
		Go.killAllTweensWithTarget( thisTransform );
		BroadcastMessage( "KillTweens" );
		Destroy( gameObject );
	}
	
	private Transform avatarTransform;
	
	// Use this for initialization
	void Start () {
		avatarTransform = GameObject.FindGameObjectWithTag( "Avatar" ).transform;
		StartCoroutine( PollAvatarPosition() );
	}
	
	public float movementForce;
	
	private void MoveToPoint ( Vector3 targetPosition ) {
		thisRigidbody.AddForce( Vector3.Normalize(  targetPosition - thisTransform.position ) * movementForce , ForceMode.Acceleration );
	}
	
	public float rotationSpeed;
	
	// Update is called once per frame
	void Update () {
		if ( isTimeStopped ) {
			thisRigidbody.velocity = Vector3.zero;
			thisRigidbody.angularVelocity = Vector3.zero;
		} else {
			MoveToPoint( bufferedAvatarPosition );
			if ( thisRigidbody.velocity.normalized.sqrMagnitude > Vector3.zero.sqrMagnitude ) thisRigidbody.rotation = Quaternion.Slerp( thisRigidbody.rotation , Quaternion.LookRotation( thisRigidbody.velocity.normalized ) , Time.deltaTime * rotationSpeed );
		}
	}

}
