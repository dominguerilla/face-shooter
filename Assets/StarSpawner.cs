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
        starball star = starball.GetComponent<starball>();
        star.SetSpawner(this);
        spawnedEntities.Add(starball);
        

        Rigidbody rb = starball.GetComponent<Rigidbody>();
        rb.AddForce(Vector3.forward * launchForce);
        star.StartInitialDespawn();
        yield return null; 
    }

    public void DeregisterStar(starball star)
    {
        GameObject obj = star.gameObject;
        spawnedEntities.Remove(obj);
    }

}
