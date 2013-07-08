using UnityEngine;
using System.Collections;

public class InvertPlaneController : MonoBehaviour {
	
	private Material thisMaterial;
	
	void Awake () {
		thisMaterial = renderer.material;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	public void StopTime ( float duration ) {
		thisMaterial.color = Color.white;
		Go.to( thisMaterial , duration , new TweenConfig().materialColor( Color.black , MaterialColorType.Color , false ).setEaseType( EaseType.ExpoIn ) );
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
