using UnityEngine;
using System.Collections;

public class EnemyCollectableController : MonoBehaviour {
	
	public GameObject collectable;
	public float collectableInterval;
	public float collectableEjectionDistance;
	public float collectableEjectionDuration;
	public float collectableInitialScale;
	public float scaleUpDuration;
	public Transform enemyContainer;
	
	private GameObject newCollectable;
	private Tween scaleUpTween;
	private Tween collectableTween;
	
	IEnumerator AddCollectable () {
		yield return new WaitForSeconds( collectableInterval );
		newCollectable = Instantiate( collectable , thisTransform.position , Quaternion.identity ) as GameObject;
		scaleUpTween = new Tween( thisTransform , scaleUpDuration , new TweenConfig().scale( maxScale , false ).setEaseType( EaseType.SineOut ) );
		scaleUpTween.setOnCompleteHandler( scaleDown => ScaleDown() );
		Go.addTween( scaleUpTween );
		newCollectable.transform.localScale = Vector3.one * collectableInitialScale;
		newCollectable.transform.parent = thisTransform.parent.parent;
		collectableTween = new Tween( newCollectable.transform , collectableEjectionDuration , new TweenConfig().position( ( thisTransform.position -  ( Vector3.Normalize( avatarTransform.position - thisTransform.position ) * collectableEjectionDistance ) ) , false ).setEaseType( EaseType.SineInOut ) );
		Go.addTween( collectableTween );
		StartCoroutine( AddCollectable () );
	}
	
	private Transform thisTransform;
	private Transform parentTransform;
	private Rigidbody thisRigidbody;
	private Vector3 minScale;
	private Vector3 maxScale;
	
	void Awake () {
		thisTransform = transform;
		parentTransform = thisTransform.parent;
		thisRigidbody = rigidbody;
		maxScale = thisTransform.localScale;
		minScale = maxScale / 2;
		thisTransform.localScale = minScale;
	}
	
	public void StopTime () {
		StopAllCoroutines();
		scaleUpTween.pause();
		collectableTween.pause();
		scaleDownTween.pause();
	}
	
	public void StartTime () {
		StartCoroutine( AddCollectable() );
		scaleUpTween.play();
		collectableTween.play();
		scaleDownTween.play();
	}
	
	public void KillTweens () {
		Go.killAllTweensWithTarget( thisTransform );
	}
	
	public float scaleDownDuration;
	
	private Tween scaleDownTween;
	
	private void ScaleDown () {
		scaleDownTween = new Tween( thisTransform , scaleDownDuration , new TweenConfig().scale( minScale , false ).setEaseType( EaseType.SineInOut ) );
		Go.addTween( scaleDownTween );
	}
	
	private Transform avatarTransform;
	
	// Use this for initialization
	void Start () {
		avatarTransform = GameObject.FindGameObjectWithTag( "Avatar" ).transform;
		StartCoroutine( AddCollectable () );
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
