using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {


	public GameObject playerPref;
	public GameObject flagPref;
	public Transform floorTransform;
	public Quaternion rotation;

	public Vector3 playerSpawnPos;
	public Vector3 flagSpawnPos;

	public float spawnRange;
	public float flagSpawnRange;
	public float playerCount;

	public float distance;


	// Use this for initialization
	void Start() {
		//PickRange ();
	//	SpawnPlayer ();
	//	SpawnFlag ();
	}
	//not useing this but save for later
	void PickRange()
	{
		playerSpawnPos = new Vector3 (Random.Range (-spawnRange, spawnRange), 5, Random.Range (-spawnRange, spawnRange));
		flagSpawnPos = new Vector3 (Random.Range (-flagSpawnRange, flagSpawnRange), 5, Random.Range (-flagSpawnRange, flagSpawnRange));		
		float dist = Vector3.Distance (playerSpawnPos, flagSpawnPos);
		while (dist < distance)
		{
			PickRange ();
		}

		if (dist > distance)
		{
			//for (int i = 0; i < playerCount; i++) {
				
				SpawnPlayer ();
				//PickRange ();
			//}
			SpawnFlag ();
		}

	}

	void SpawnPlayer()
	{
		GameObject player = GameObject.Instantiate (playerPref, playerSpawnPos,rotation)as GameObject;

	}

	void SpawnFlag()
	{
		GameObject flag = GameObject.Instantiate (flagPref,flagSpawnPos,rotation)as GameObject;
	
	}


	// Update is called once per frame
	void Update () {

		//SpawnPlayer ();
	}
}
