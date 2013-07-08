using UnityEngine;
using System.Collections;

public class CursorController : MonoBehaviour {
	
	public GameObject currentAvatar;
	
	void OnTriggerEnter ( Collider hitCollider ) {
		if ( isTimeStopped && hitCollider.transform.tag == "Enemy"  ) {
			hitCollider.transform.SendMessage( "AddTag" , SendMessageOptions.DontRequireReceiver );
			currentAvatar.SendMessage( "AddToTaggedList" , hitCollider.gameObject , SendMessageOptions.DontRequireReceiver );
		}
	}
	
	void Awake () {
		Screen.showCursor = false;
	}
	
	private Transform thisTransform;
	private Transform mainCamera;
	private Vector3 initialScale;
	
	// Use this for initialization
	void Start () {
		thisTransform = transform;
		initialScale = thisTransform.localScale;
		mainCamera = GameObject.FindGameObjectWithTag( "MainCamera" ).transform;
	}
	
	public float scaleUpDuration;
	public Vector3 StopTimeScale;
	private bool isTimeStopped;
	
	public void StopTime () {
		isTimeStopped = true;
		Go.to( thisTransform , scaleUpDuration , new TweenConfig().scale( StopTimeScale , false ).setEaseType( EaseType.ExpoOut ) );
	}
	
	public float scaleDownDuration;
	
	public void StartTime () {
		isTimeStopped = false;
		Go.to( thisTransform , scaleDownDuration , new TweenConfig().scale( initialScale , false ).setEaseType( EaseType.ExpoIn ) );
	}
	
	public static float cursorZDepth = 0f;
	
	private Vector3 mousePosition;
	
	// Update is called once per frame
	void Update () {
		mousePosition = mainCamera.camera.ScreenToWorldPoint( Input.mousePosition );
		mousePosition.z = cursorZDepth;
		thisTransform.position = mousePosition;
	}
}
