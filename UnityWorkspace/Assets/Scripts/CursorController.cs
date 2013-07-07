using UnityEngine;
using System.Collections;

public class CursorController : MonoBehaviour {
	
	void Awake () {
		Screen.showCursor = false;
	}
	
	private Transform thisTransform;
	private Transform mainCamera;
	
	// Use this for initialization
	void Start () {
		thisTransform = transform;
		mainCamera = GameObject.FindGameObjectWithTag( "MainCamera" ).transform;
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
