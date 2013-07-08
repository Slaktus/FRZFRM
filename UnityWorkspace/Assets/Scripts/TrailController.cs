using UnityEngine;
using System.Collections;

public class TrailController : MonoBehaviour {
	
	public float detachedDuration;
	
	IEnumerator CleanUpTrail () {
		yield return new WaitForSeconds( detachedDuration );
		Destroy( gameObject );
	}
	
	public void DetachTrail () {
		transform.parent = transform.parent.parent;
		StartCoroutine( CleanUpTrail() );
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
