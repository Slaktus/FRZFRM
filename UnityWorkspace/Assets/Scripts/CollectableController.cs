using UnityEngine;
using System.Collections;

public class CollectableController : MonoBehaviour {
	
	public float destroyTweenDuration;
	public Vector3 minScale;
	
	private bool isChasingAvatar;
	private Transform avatarTransform;
	
	void OnTriggerEnter ( Collider collider ) {
		if ( !isTimeStopped && collider.transform.tag == "OuterRim" && thisTransform.localScale.sqrMagnitude > minScale.sqrMagnitude ) {
			isChasingAvatar = true;
			avatarTransform = collider.transform.parent;
			Go.killAllTweensWithTarget( thisTransform );
		}
		if ( !isTimeStopped && collider.transform.tag == "Avatar" ) {
			collider.transform.SendMessageUpwards( "ScaleUpInnerCircle" , SendMessageOptions.DontRequireReceiver );
			Go.killAllTweensWithTarget( thisTransform );
			Go.to( thisTransform , destroyTweenDuration , new TweenConfig().scale( Vector3.zero , false ).setEaseType( EaseType.ExpoOut ) ).setOnCompleteHandler( destroy => Destroy( gameObject ) );
			
		}
	}
	
	private Transform thisTransform;
	private TrailRenderer thisTrailRenderer;
	private GameObject thisTrail;
	
	void Awake () {
		thisTransform = transform;
		thisTrail = transform.FindChild( "Trail" ).gameObject;
		thisTrailRenderer = thisTrail.GetComponent< TrailRenderer >();
	}
	
	public float quickScaleDownDuration;
	
	private bool isTimeStopped;
	
	public void StopTime () {
		isTimeStopped = true;
		Go.killAllTweensWithTarget( thisTransform );
		Go.to ( thisTransform , quickScaleDownDuration , new TweenConfig().scale( Vector3.zero , false ).setEaseType( EaseType.ExpoOut ) ).setOnCompleteHandler( cleanUp => CleanUpCollectable() );
	}
	
	public float collectableScaleDownDuration;
	
	private float scaleDownStartTime;
	
	private void ScaleDownTween() {
		scaleDownStartTime = Time.time;
		currentTween = new Tween( thisTransform , collectableScaleDownDuration , new TweenConfig().scale( Vector3.zero , false ).setEaseType( EaseType.ExpoInOut ) );
		currentTween.setOnCompleteHandler( cleanUp => CleanUpCollectable() );
		Go.addTween( currentTween );
	}
	
	private void CleanUpCollectable() {
		Go.killAllTweensWithTarget( thisTransform );
		thisTrail.SendMessage( "DetachTrail" );
		Destroy( gameObject );
	}
	
	public float collectableScaleUpDuration;
	
	private Vector3 initialScale;
	private Tween currentTween;
	
	// Use this for initialization
	void Start () {
		initialScale = transform.localScale;
		thisTransform.localScale = Vector3.zero;
		currentTween = new Tween( thisTransform , collectableScaleUpDuration , new TweenConfig().scale( initialScale , false ).setEaseType( EaseType.ExpoOut ) );
		currentTween.setOnCompleteHandler( scaleDown => ScaleDownTween() );
		Go.addTween( currentTween );
	}
	
	public float avatarChaseSpeed;
	
	// Update is called once per frame
	void Update () {
		if ( !isTimeStopped && isChasingAvatar ) {
			if ( !thisTrailRenderer.enabled ) thisTrailRenderer.enabled = true;
			else thisTrailRenderer.startWidth = thisTransform.localScale.x;
			thisTransform.position = Vector3.Slerp( thisTransform.position , avatarTransform.position , Time.deltaTime * avatarChaseSpeed );
			
		}
	}
}
