using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootSpeedPowerUp : MonoBehaviour {


	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player")
		{
			if (col.GetComponent<PlayerController> ().hasFlag == true) {
				Debug.Log ("Flag PowerUp Scored");
				col.GetComponent<PlayerController> ().m_linearSpeed += 5;
			}
				else {
					Debug.Log ("Shoot Speed PowerUP Scored");
				col.GetComponent<PlayerController> ().fireRate -= 0.5f;
				}
			Destroy (gameObject);
		}

	}
}
