using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSpawner : EntitySpawner {

    public float launchForce = 5.0f;

    private void Start()
    {
        if (activateOnStart)
            StartSpawning();
    }


    public override IEnumerator SpawnEntity()
    {
        GameObject starball = Instantiate(Entities[0], this.transform.position + (spawnDisplacement), this.transform.rotation);
        spawnedEntities.Add(starball);

        Rigidbody rb = starball.GetComponent<Rigidbody>();
        rb.AddForce(Vector3.forward * launchForce);
        yield return null; 
    }

}
