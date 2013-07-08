using UnityEngine;
using System.Collections;

public class AvatarScaleController : MonoBehaviour {
	
	public float timeStopDuration;
	public float ScaleDownDurationPadding = 0.2f;
	
	IEnumerator LaunchProjectiles () {
		bufferedScale = Vector3.one * 2;
		Go.to( innerCircleTransform , timeStopDuration - ScaleDownDurationPadding , new TweenConfig().scale( bufferedScale , false ).setEaseType( EaseType.Linear ) );
		gameObject.SendMessage( "StartSlowMo" );
		yield return new WaitForSeconds( timeStopDuration );
		gameObject.SendMessage( "FireProjectiles" );
	}
	
	private Transform thisTransform;
	private GameObject enemyContainer;
	private GameObject uiContainer;
	private GameObject cursor;
	private TrailRenderer mainTrail;
	
	void Awake () {
		thisTransform = transform;
		enemyContainer = GameObject.FindGameObjectWithTag( "EnemyContainer" );
		uiContainer = GameObject.FindGameObjectWithTag( "UIContainer" );
		cursor = GameObject.FindGameObjectWithTag( "Cursor" );
		bufferedScale = innerCircleTransform.localScale;
		mainTrail = thisTransform.FindChild( "OuterTrail" ).gameObject.GetComponent< TrailRenderer >();
	}
	
	public Transform innerCircleTransform;
	public Transform outerRimTransform;
	
	// Use this for initialization
	void Start () {
	
	}
	
	public float scaleUpInnerCircleDuration;
	public Vector3 scaleIncrement;
	public float maxScaleOverflow;
	
	private Tween ScaleUpInnerCircleTween;
	private TweenChain ScaleUpInnerCircleChain;
	private Vector3 bufferedScale;
	
	public void ScaleUpInnerCircle () {
		if ( bufferedScale.sqrMagnitude < outerRimTransform.localScale.sqrMagnitude ) {
			bufferedScale += scaleIncrement;
			if ( bufferedScale.sqrMagnitude >= outerRimTransform.localScale.sqrMagnitude ) bufferedScale = outerRimTransform.localScale + ( Vector3.one * maxScaleOverflow );
			Go.killAllTweensWithTarget( innerCircleTransform );
			Go.to( innerCircleTransform , scaleUpInnerCircleDuration , new TweenConfig().scale( bufferedScale , false ).setEaseType( EaseType.BackOut ) ).setOnCompleteHandler( check => CheckInnerCircleScale() );
			AddScaleUpEffect();
		}
	}
	
	public GameObject scaleUpParticles;
	
	private GameObject scaleUpEffect;
	
	private void AddScaleUpEffect () {
		scaleUpEffect = Instantiate( scaleUpParticles , innerCircleTransform.position , Quaternion.identity ) as GameObject;
		scaleUpEffect.transform.parent = thisTransform;
	}
	
	public void RestartTime () {
		isTimeStopped = false;
		enemyContainer.BroadcastMessage( "StartTime" , SendMessageOptions.DontRequireReceiver );
		cursor.SendMessage( "StartTime" );
		gameObject.GetComponent< AvatarMovementController >().enabled = true;
	}
	
	private bool isTimeStopped;
	
	private void CheckInnerCircleScale () {
		if ( !isTimeStopped && innerCircleTransform.localScale.sqrMagnitude > outerRimTransform.localScale.sqrMagnitude ) {
			if ( gameObject.GetComponent< AvatarMovementController >().enabled ) gameObject.GetComponent< AvatarMovementController >().enabled = false;
			rigidbody.velocity = Vector3.zero;
			enemyContainer.BroadcastMessage( "StopTime" , SendMessageOptions.DontRequireReceiver);
			uiContainer.BroadcastMessage( "StopTime" , timeStopDuration , SendMessageOptions.DontRequireReceiver );
			cursor.SendMessage( "StopTime" , SendMessageOptions.DontRequireReceiver );
			isTimeStopped = true;
			StartCoroutine( LaunchProjectiles() );
		}	
	}
	
	
	// Update is called once per frame
	void Update () {
		if ( isTimeStopped ) rigidbody.velocity = Vector3.zero;
		mainTrail.startWidth = innerCircleTransform.localScale.x;
	}
}
