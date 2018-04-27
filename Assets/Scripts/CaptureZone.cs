using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CaptureZone : NetworkBehaviour {

	public GameObject Gm;

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player")
		{
			if(col.GetComponent<PlayerController>().hasFlag == true)
			{
				if (col.GetComponent<PlayerController> ().isServer == true) {
					Debug.Log ("Host Scored");
					Gm.GetComponent<GameManager> ().RespawnFlag ();
				} else {
					Debug.Log ("Client Scored");
					Gm.GetComponent<GameManager> ().RespawnFlag ();
				}
			}
		}

	}
}
