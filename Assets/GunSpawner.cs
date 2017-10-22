using System.Collections;
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

	void Start () {
        spawnedGuns = new List<Gun>();
        if (activateOnStart)
            StartSpawning();
	}

    /// <summary>
    /// Called by a gun being despawned, in order to remove itself from the spawned gun list.
    /// </summary>
    /// <notes>
    /// This way, the spawner won't wait extra time while the gun is being despawned.
    /// </notes>
    /// <param name="gun"></param>
    public void DeregisterGun(Gun gun)
    {
        spawnedGuns.Remove(gun);
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
            if(spawnedGuns.Count != maxGunsSpawned)
                StartCoroutine(SpawnGun());
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    IEnumerator SpawnGun()
    {   
        int index = Random.Range(0, GunPrefabs.Length);
        GameObject gunObj = Instantiate(GunPrefabs[index], this.transform.position + (Vector3.up * spawnDisplacement), this.transform.rotation);
        Gun gun = gunObj.GetComponent<Gun>();
        gun.SetSpawner(this);
        spawnedGuns.Add(gun);
        Rigidbody gunBody = gunObj.GetComponent<Rigidbody>();
        yield return new WaitForFixedUpdate();
        float deltaX = Random.Range(-maxTrajectoryDeviation.x, maxTrajectoryDeviation.x);
        float deltaY = Random.Range(-maxTrajectoryDeviation.y, maxTrajectoryDeviation.y);
        float deltaZ = Random.Range(-maxTrajectoryDeviation.z, maxTrajectoryDeviation.z);
        Vector3 deviation = new Vector3(deltaX, deltaY, deltaZ);
        gunBody.AddForce(transform.TransformDirection(Vector3.up + deviation) * launchForce);
    }
}
