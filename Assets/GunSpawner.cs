using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSpawner : MonoBehaviour {

    public GameObject GunPrefab;
    public float launchForce = 5.0f;
    public float timeBetweenSpawns = 3.0f;
    public float spawnDisplacement = 1.0f;

	// Use this for initialization
	void Start () {
        StartCoroutine(SpawnGuns());	
	}
	

    IEnumerator SpawnGuns()
    {
        while (true)
        {
            StartCoroutine(SpawnGun());
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    IEnumerator SpawnGun()
    {
        GameObject gunObj = Instantiate(GunPrefab, this.transform.position + (Vector3.up * spawnDisplacement), this.transform.rotation);
        Rigidbody gunBody = gunObj.GetComponent<Rigidbody>();
        yield return new WaitForFixedUpdate();
        gunBody.AddForce(transform.TransformDirection(Vector3.up) * launchForce);
    }
}
