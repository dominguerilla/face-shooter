using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSpawner : MonoBehaviour {

    public GameObject GunPrefab;
    public float launchForce = 5.0f;
    public float timeBetweenSpawns = 3.0f;
    public float spawnDisplacement = 1.0f;
    public float maxGunsSpawned = 1;

    List<Gun> spawnedGuns;
    bool started;

	// Use this for initialization
	void Start () {
        spawnedGuns = new List<Gun>();	
	}

    private void Update()
    {
        foreach(Gun gun in spawnedGuns)
        {
            if (gun.isEmptyOrDropped())
            {
                spawnedGuns.Remove(gun);
                Destroy(gun.gameObject, 2.0f);
                break;
            }
        }
    }

    public void StartSpawning()
    {
        if (!started)
        {
            Debug.Log("Started spawning guns!");
            StartCoroutine(SpawnGuns());
        }
    }

    IEnumerator SpawnGuns()
    {
        started = true;
        while (true)
        {
            if(spawnedGuns.Count < maxGunsSpawned)
                StartCoroutine(SpawnGun());
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    IEnumerator SpawnGun()
    {
        GameObject gunObj = Instantiate(GunPrefab, this.transform.position + (Vector3.up * spawnDisplacement), this.transform.rotation);
        Gun gun = gunObj.GetComponent<Gun>();
        spawnedGuns.Add(gun);
        Rigidbody gunBody = gunObj.GetComponent<Rigidbody>();
        yield return new WaitForFixedUpdate();
        gunBody.AddForce(transform.TransformDirection(Vector3.up) * launchForce);
    }
}
