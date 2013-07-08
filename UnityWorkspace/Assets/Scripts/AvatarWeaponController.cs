using UnityEngine;
using System.Collections;

public class AvatarWeaponController : MonoBehaviour {
	
	public float launchSlowMoDuration;
	
	IEnumerator EndSlowMo () {
		yield return new WaitForSeconds( launchSlowMoDuration );
		Time.timeScale = 1f;
		Time.fixedDeltaTime = 0.02f;
		gameObject.SendMessage( "RestartTime" );
	}
	
	private Transform thisTransform;
	
	void Awake () {
		thisTransform = transform;
	}
	
	private ArrayList taggedEnemyList = new ArrayList();
	
	public void AddToTaggedList ( GameObject taggedEnemy ) {
		taggedEnemyList.Add( taggedEnemy );
	}
	
	public GameObject projectile;
	public float launchTimeScale = 0.5f;
	public float launchFixedDelta = 0.01f;
	
	private GameObject newProjectile;
	private ProjectileController projectilescript;
	private int numberOfEnemies;
	private float angleIncrement;
	private float currentAngle;
	
	public void FireProjectiles () {
		currentAngle = 0;
		numberOfEnemies = taggedEnemyList.Count;
		if ( numberOfEnemies > 0 ) {
			StartCoroutine( EndSlowMo() );
			angleIncrement = 360 / ( numberOfEnemies * 3 );
			foreach ( GameObject enemy in taggedEnemyList ) {
				for ( int i = 0; i < 3 ; i++ ) {
					currentAngle += angleIncrement;
					newProjectile = Instantiate( projectile , thisTransform.position , Quaternion.identity ) as GameObject;
					newProjectile.transform.parent = thisTransform.parent;
					projectilescript = newProjectile.GetComponent< ProjectileController >();
					projectilescript.targetEnemy = ( GameObject ) taggedEnemyList[ numberOfEnemies - 1 ];
					if ( numberOfEnemies % 2 == 0 ) projectilescript.ejectionDirection = Quaternion.AngleAxis( currentAngle , Vector3.forward ) * Vector3.left;
					else projectilescript.ejectionDirection = Quaternion.AngleAxis( currentAngle - 225 , Vector3.forward ) * Vector3.left;
				}
				numberOfEnemies -= 1;
			}
		} else {
			Time.timeScale = 1f;
			Time.fixedDeltaTime = 0.02f;
			gameObject.SendMessage( "RestartTime" );
		}
		if (taggedEnemyList.Count > 0 ) taggedEnemyList.Clear();
	}
	
	public void StartSlowMo () {
		Time.timeScale = launchTimeScale;
		Time.fixedDeltaTime = launchFixedDelta;
	}
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
