using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class Timer : NetworkBehaviour {

	public Text timer;

	public const float maxTime = 100;
	[SyncVar(hook = "OnChangeTime")]
	public float currentTime = maxTime;

	public GameObject endGamePanel;

	public GameObject powerUpPref;

	public Transform powerUpSpawnPos;

	private bool DoOnce = false;

	bool gameStarted = false;
	// Use this for initialization
	void Start () {
		timer.gameObject.SetActive (true);	
	}

	public void ReduceTime(float amount)
	{
		if (!isServer)
		{
			return;
		}

		currentTime -= amount;

		if (currentTime <= 0.0f)
		{
			EndGameResults ();
		}

		if (currentTime <= 30.0f &&!DoOnce)
		{
			DoOnce = true;
			SpawnPowerUp ();
		}
	}


	public void SpawnPowerUp ()
	{
		GameObject powerUp = GameObject.Instantiate (powerUpPref, powerUpSpawnPos)as GameObject;
		NetworkServer.Spawn (powerUp);

	}

	public void EndGameResults()
	{
		endGamePanel.SetActive (true);
		RpcEndGame ();
	}

	[ClientRpc]
	public void RpcEndGame()
	{
		endGamePanel.SetActive (true);
	}
		
	public void StartTimer()
	{
		gameStarted = true;
	}

	public void RestartTimer()
	{
		currentTime = maxTime;
	}

	void OnChangeTime(float currentTime)
	{
		timer.text = currentTime.ToString ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(gameStarted)
		ReduceTime (0.01f);
	}
}
