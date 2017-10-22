﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSpawner : MonoBehaviour {

    public bool activateOnStart = false;
    public GameObject[] GunPrefabs;
    public float launchForce = 5.0f;
    public float timeBetweenSpawns = 3.0f;
    public float spawnDisplacement = 1.0f;
    public float maxGunsSpawned = 1;
    public Vector3 maxTrajectoryDeviation = new Vector3(0.1f, 0.1f, 0.1f);

    List<Gun> spawnedGuns;
    bool started;


	// Use this for initialization
	void Start () {
        spawnedGuns = new List<Gun>();
        if (activateOnStart)
            StartSpawning();
	}

    private void Update()
    {
        foreach(Gun gun in spawnedGuns)
        {
            if (gun.isEmptyOrDropped() || 
                (!gun.isEquipped() && !movedSinceLastCheck(gun.gameObject)))
            {
                DespawnGun(gun);
                break;
            }
        }
    }

    public void StartSpawning()
    {
        if (!started)
        {
            StartCoroutine(SpawnGuns());
        }
    }

    IEnumerator SpawnGuns()
    {
        started = true;
        while (true)
        {
            if(maxGunsSpawned < 0 || spawnedGuns.Count < maxGunsSpawned)
                StartCoroutine(SpawnGun());
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    IEnumerator SpawnGun()
    {   
        int index = Random.Range(0, GunPrefabs.Length);
        GameObject gunObj = Instantiate(GunPrefabs[index], this.transform.position + (Vector3.up * spawnDisplacement), this.transform.rotation);
        Gun gun = gunObj.GetComponent<Gun>();
        spawnedGuns.Add(gun);
        Rigidbody gunBody = gunObj.GetComponent<Rigidbody>();
        yield return new WaitForFixedUpdate();
        float deltaX = Random.Range(-maxTrajectoryDeviation.x, maxTrajectoryDeviation.x);
        float deltaY = Random.Range(-maxTrajectoryDeviation.y, maxTrajectoryDeviation.y);
        float deltaZ = Random.Range(-maxTrajectoryDeviation.z, maxTrajectoryDeviation.z);
        Vector3 deviation = new Vector3(deltaX, deltaY, deltaZ);
        gunBody.AddForce(transform.TransformDirection(Vector3.up + deviation) * launchForce);
    }


    // returns true if the gun moved since the last check
    bool movedSinceLastCheck(GameObject gun)
    {
        PositionTracker tracker = gun.GetComponent<PositionTracker>();
        if (tracker != null && tracker.lastCheckedPosition != gun.transform.position)
        {
            return true;
        }
        return false;
    }

    void DespawnGun(Gun gun)
    {
        spawnedGuns.Remove(gun);
        Destroy(gun.gameObject, 2.0f);
    }
}
