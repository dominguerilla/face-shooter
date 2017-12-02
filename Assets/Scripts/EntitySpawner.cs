using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntitySpawner : MonoBehaviour {

    public GameObject[] SpawnEntities;
    public bool activateOnStart;
    public float timeBetweenSpawns;
    public int maxEntitiesSpawned = 1;

    public abstract void StartSpawning();
    public virtual void ResetSpawner()
    {

    }
}
