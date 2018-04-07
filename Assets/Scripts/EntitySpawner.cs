using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class EntitySpawner : MonoBehaviour {

    /// <summary>
    /// The entities to spawn from this spawner.
    /// </summary>
    public GameObject[] Entities;

    /// <summary>
    /// The list of currently spawned entities.
    /// </summary>
    [HideInInspector]
    public List<GameObject> spawnedEntities;

    /// <summary>
    /// Whether or not this spawner should activate on scene start. Must be implemented.
    /// </summary>
    public bool activateOnStart;

    /// <summary>
    /// The time between successive spawns of the entities.
    /// </summary>
    public float timeBetweenSpawns;

    /// <summary>
    /// The max number of entities spawned at a time.
    /// </summary>
    public int maxEntitiesSpawned = 1;

    /// <summary>
    /// Where the entity should be spawned relative to the spawner.
    /// </summary>
    public Vector3 spawnDisplacement;

    /// <summary>
    /// True if the spawner started spawning.
    /// </summary>
    public bool started;

    /// <summary>
    /// Called whenever an entity is spawned.
    /// </summary>
    public UnityEvent OnSpawn;

    /// <summary>
    /// Time delay before spawning the first object.
    /// </summary>
    public float spawnDelay = 0.0f;

    /// <summary>
    /// Called by any external object to make the spawner start spawning.
    /// Usually, calls some private IEnumerator method that repeatedly calls SpawnEntity().
    /// </summary>
    public virtual void StartSpawning()
    {
        if (!started)
        {
            StartCoroutine(SpawnEntities());
        }
    }

    public virtual IEnumerator SpawnEntities()
    {
        started = true;

        yield return new WaitForSeconds(spawnDelay);
        while (true)
        {
            if (spawnedEntities.Count != maxEntitiesSpawned)
            {
                StartCoroutine(SpawnEntity());
                OnSpawn.Invoke();
            }
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    public abstract IEnumerator SpawnEntity();

    public virtual void ResetSpawner()
    {

    }
}
