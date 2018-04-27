using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {

	public int MaxPlayers = 3;
	public int players = 0;
	public GameObject spawner;
	public GameObject timer;
	// Use this for initialization
	private bool DoOnce = false;
	private bool Restart = false;

	public GameObject m_flagPrefab = null;
	public Transform flagSpawnPos;
	void Start () {
		
	}


	
	// Update is called once per frame
	void Update () {

		Debug.Log ("players = " + players);
		if (players == MaxPlayers && !DoOnce)
		{
			if (isServer) {
				SpawnFlag ();
			}
			timer.GetComponent<Timer> ().StartTimer ();
			DoOnce = true;
		}

//		if (players > MaxPlayers)
//		{
//			Restart = true;
//			if (Restart)
//			{
//				
//				if (isServer) 
//				{
////					GameObject flag =  GameObject.FindGameObjectWithTag ("flag");
////					NetworkServer.Destroy (flag);
////					SpawnFlag ();
//					MaxPlayers++;
//				}
//				timer.GetComponent<Timer> ().RestartTimer ();
//				Restart = false;
//			}
//		}
	}


	public void SpawnFlag()
	{
		//RpcSpawnObject(m_flagPrefab, m_range);
		GameObject flag = GameObject.Instantiate (m_flagPrefab, flagSpawnPos)as GameObject;
		NetworkServer.Spawn (flag);

	}

	public void RespawnFlag()
	{
		GameObject oldFlag =  GameObject.FindGameObjectWithTag ("flag");
		NetworkServer.Destroy (oldFlag);
		GameObject flag = GameObject.Instantiate (m_flagPrefab, flagSpawnPos)as GameObject;
		NetworkServer.Spawn (flag);

	}
}
