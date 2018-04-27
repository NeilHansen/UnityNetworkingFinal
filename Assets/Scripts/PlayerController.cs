using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomMsgType
{
    public static short Transform = MsgType.Highest + 1;
};


public class PlayerController : NetworkBehaviour
{
    public float m_linearSpeed = 5.0f;
    public float m_angularSpeed = 3.0f;

    private Rigidbody m_rb = null;
	//public Transform handOffset;
	private Vector3 handOffset;
	public bool hasFlag = false;
	private GameObject GC;
	bool playerJoined;

	public GameObject bulletPref;
	public Transform bulletSpwn;

	public int ammo = 3;
	public float fireRate; 
	private float nextFire;

    bool IsHost()
    {
        return isServer && isLocalPlayer;
    }

    // Use this for initialization
    void Start () {
        m_rb = GetComponent<Rigidbody>();
        Debug.Log("Start()");
		GC = GameObject.FindGameObjectWithTag ("GameController");
		playerJoined = true;
      //  Vector3 spawnPoint;
       //ObjectSpawner.RandomPoint(this.transform.position, 10.0f, out spawnPoint);
       //this.transform.position = spawnPoint;

//		GC.GetComponent<ObjectSpawner> ().player++;
        TrailRenderer tr = GetComponent<TrailRenderer>();
        tr.enabled = false;
	}

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        Debug.Log("OnStartAuthority()");
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        Debug.Log("OnStartClient()");
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        Debug.Log("OnStartLocalPlayer()");
        GetComponent<MeshRenderer>().material.color = new Color(0.0f, 2.0f, 0.0f);

    }



    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("OnStartServer()");

    }


	[Command]
	public void CmdJump()
	{
		RpcJump();
		Jump();
	}

    [ClientRpc]
    public void RpcJump()
    {
        Jump();
    }


	public void Jump()
	{
		Vector3 jumpVelocity = Vector3.up * 5.0f;
		m_rb.velocity += jumpVelocity;
		TrailRenderer tr = GetComponent<TrailRenderer>();
		tr.enabled = true;
	}


	[Command]
	public void CmdPickUp(GameObject flag)
	{
		RpcPickUp(flag);
		PickUp(flag);
	}

	[ClientRpc]
	public void RpcPickUp(GameObject flag)
	{
		PickUp(flag);
	}


	public void PickUp(GameObject flag)
	{
		Debug.Log ("Pick Up");
		flag.transform.parent = this.transform.GetChild(1).transform;
		handOffset = this.transform.GetChild(1).transform.position + new Vector3(0,2.5f,0);
		flag.transform.position = handOffset;
		hasFlag = true;
		m_linearSpeed -= 5;
		flag.GetComponent<ParticleSystem> ().Stop ();
	}

	[Command]
	public void CmdDropFlag()
	{
		RpcDropFlag();
		DropFlag();
	}

	[ClientRpc]
	public void RpcDropFlag()
	{
		DropFlag();
	}

	public void DropFlag()
	{
		if (hasFlag) {
			this.transform.GetChild (1).GetChild (0).GetComponent<ParticleSystem> ().Play ();
			this.transform.GetChild (1).DetachChildren ();
			m_linearSpeed += 5;
			hasFlag = false;
		}
	}

	void OnTriggerStay(Collider col)
	{
		
		if (col.gameObject.tag == "flag")
		{
			
			if (Input.GetKey (KeyCode.E)) 
			{
				if (!hasFlag)
				{
					if(isLocalPlayer)
					CmdPickUp (col.gameObject);
				}
			}
		}
	}

	[Command]
	void CmdShoot()
	{
		//make bullet
		var bulletClone = (GameObject)Instantiate(bulletPref, bulletSpwn.position, bulletSpwn.rotation);

		//add velocity to shoot
		bulletClone.GetComponent<Rigidbody>().velocity = bulletClone.transform.forward * 20;

		NetworkServer.Spawn (bulletClone);
		Destroy (bulletClone, 2.0f);
	}

    // Update is called once per frame
    void Update () {
		if (playerJoined) {
			GC.GetComponent<GameManager>().players++;
			playerJoined = false;
		}

        if(!isLocalPlayer)
        {
            return;
        }

        float rotationInput = Input.GetAxis("Horizontal");
        float forwardInput = Input.GetAxis("Vertical");

        Vector3 linearVelocity = this.transform.forward * (forwardInput * m_linearSpeed);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            CmdJump();
        }
		if(Input.GetKeyDown(KeyCode.LeftControl) && Time.time > nextFire)
		{
			nextFire = Time.time + fireRate;
			CmdShoot();
		}
			
        float yVelocity = m_rb.velocity.y;


        linearVelocity.y = yVelocity;
        m_rb.velocity = linearVelocity;

        Vector3 angularVelocity = this.transform.up * (rotationInput * m_angularSpeed);
        m_rb.angularVelocity = angularVelocity;


    }

	void OnDestroy()
	{
		Debug.Log ("destroyed");
		//this.gameObject.transform.GetChild (1).DetachChildren ();
		CmdDropFlag ();
	}
   
}
