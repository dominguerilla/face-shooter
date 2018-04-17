using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSpawner : EntitySpawner {

    public float launchForce = 5.0f;
    public Vector3 maxTrajectoryDeviation = new Vector3(0.1f, 0.1f, 0.1f);
    public FaceEnemy.COLOR affinity = FaceEnemy.COLOR.NONE;


	void Start () {
        spawnedEntities = new List<GameObject>();
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
        spawnedEntities.Remove(gun.gameObject);
    }


    public override IEnumerator SpawnEntity()
    {   
        int index = UnityEngine.Random.Range(0, Entities.Length);
        GameObject gunObj = Instantiate(Entities[index], this.transform.position + spawnDisplacement, UnityEngine.Random.rotation);

        // Gun/Gun Spawner initialization
        Gun gun = gunObj.GetComponent<Gun>();
        gun.SetSpawner(this);
        if (affinity != FaceEnemy.COLOR.NONE)
            gun.SetAffinity(affinity);
        spawnedEntities.Add(gun.gameObject);

        // Launching the gun with a relatively random direction
        Rigidbody gunBody = gunObj.GetComponent<Rigidbody>();
        yield return new WaitForFixedUpdate();
        float deltaX = UnityEngine.Random.Range(-maxTrajectoryDeviation.x, maxTrajectoryDeviation.x);
        float deltaY = UnityEngine.Random.Range(-maxTrajectoryDeviation.y, maxTrajectoryDeviation.y);
        float deltaZ = UnityEngine.Random.Range(-maxTrajectoryDeviation.z, maxTrajectoryDeviation.z);
        Vector3 deviation = new Vector3(deltaX, deltaY, deltaZ);
        gunBody.AddForce(transform.TransformDirection(Vector3.up + deviation) * launchForce);

        // Adding a relatively random spin to the gun
        deltaX = UnityEngine.Random.Range(-maxTrajectoryDeviation.x, maxTrajectoryDeviation.x);
        deltaY = UnityEngine.Random.Range(-maxTrajectoryDeviation.y, maxTrajectoryDeviation.y);
        deltaZ = UnityEngine.Random.Range(-maxTrajectoryDeviation.z, maxTrajectoryDeviation.z);
        deviation = new Vector3(deltaX, deltaY, deltaZ);
        gunBody.AddRelativeTorque(deviation * launchForce);
    }


}
