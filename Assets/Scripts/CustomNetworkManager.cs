using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager {

	public NetworkManager nManager;
	// Use this for initialization


	public void OnHostStart()
	{
		base.StartHost();
	}

	public void OnClientStart()
	{
		base.StartClient();
	}

	public void DisconnectHost()
	{
		base.StopHost ();
	}

	public void DisconnectClient()
	{
		base.StopClient ();
	}

	

}
