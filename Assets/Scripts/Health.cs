﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {
	
	public const int maxHealth = 100;
	[SyncVar(hook = "OnChangeHealth")]
	public int currentHealth = maxHealth;

	public RectTransform healthBar;

	public bool destroyOnDeath;

	private NetworkStartPosition[] spawnPoints;


	void Start()
	{
		if (isLocalPlayer)
		{
			spawnPoints = FindObjectsOfType<NetworkStartPosition> ();
		}
	}


	public void TakeDamage(int amount)
	{
		if (!isServer) {
			return;
		}


		currentHealth -= amount;
		if (currentHealth <= 0)
		{
			if (destroyOnDeath) {
				Destroy (gameObject);
				Debug.Log ("Enemy Died!");
			}
			else
			{
				RpcRespawn ();
				currentHealth = maxHealth;
				Debug.Log ("Player Died!");
				this.GetComponent<PlayerController> ().CmdDropFlag ();

			}
		}

	}

	void OnChangeHealth(int currentHealth)
	{
		healthBar.sizeDelta = new Vector2 (currentHealth, healthBar.sizeDelta.y);
	}

	[ClientRpc]
	void RpcRespawn()
	{
		if (isLocalPlayer)
		{
			Vector3 spawnPoint = Vector3.zero;
			if (spawnPoints != null && spawnPoints.Length > 0)
			{
				spawnPoint = spawnPoints[Random.Range (0, spawnPoints.Length)].transform.position;
			}
			transform.position = spawnPoint;
		}

	}
}
