using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.Networking;



/// <summary>
/// //NOT USING
/// </summary>
public class ObjectSpawner : NetworkBehaviour {

    public GameObject m_playerPrefab = null;
  
    public Text m_timerText = null;

    public float m_range = 10.0f;

	public Quaternion rotation;


    public static bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector2 randPoint = Random.insideUnitCircle;
            Vector3 randomPoint = center + new Vector3(randPoint.x,randPoint.y,center.z) * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    public bool RpcSpawnObject(GameObject objectToSpawn, float range)
    {
        Vector3 spawnPoint;
        if (RandomPoint(new Vector3(0.0f, 0.0f, 0.0f), range, out spawnPoint))
        {
            Quaternion rotation = objectToSpawn.transform.rotation;
            var clone = (GameObject)Instantiate(objectToSpawn, spawnPoint, rotation);
			NetworkServer.Spawn (clone);
            return true;
        }
        
        Debug.Log("Could not find point to spawn");
        return false;
    }

    // Use this for initialization
    void Start ()
    {
        //SpawnObject(m_playerPrefab);
       
    }


	
	// Update is called once per frame
	void Update () {

	}
}
