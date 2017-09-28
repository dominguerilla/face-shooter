using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Spawns a prefab, and sets a Navmesh target if a target is specified and if the prefab has a NavmeshAgent.
/// </summary>
public class SpawnPoint : MonoBehaviour {

    public Transform target;
    public GameObject spawnPrefab;

    public int numberOfSpawns = -1;  // the number of times the spawn prefab will be spawned; if -1, will spawn infinitely.
    public float timeBetweenSpawns = 10.0f; // the amount of time in seconds between the spawning of prefabs.

    public bool StartOnWake;

    bool started = false;
    IEnumerator spawnRoutine;

    private void Start()
    {
        spawnRoutine = StartSpawnCycle();
        if (StartOnWake)
            StartSpawning();
    }
    
    public void StartSpawning()
    {
        if (!started)
        {
            started = true;
            StartCoroutine(spawnRoutine);
        }
    }

    public void ResetSpawn()
    {
        if (started)
        {
            started = false;
            StopCoroutine(spawnRoutine);
            Debug.Log("Stopped spawn cycle.");
        }
    }

    IEnumerator StartSpawnCycle()
    {
        int count = 0;
        while(count != numberOfSpawns)
        {
            Spawn();
            count++;
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
        started = false;
    }

    private void Spawn()
    {
        GameObject spawned = Instantiate(spawnPrefab, transform.position, transform.rotation) as GameObject;
        Monster monster = spawned.GetComponent<Monster>();
        if (monster)
        {
            monster.SetTarget(target);
            monster.StartBehavior();
        }else
        {
            Debug.Log("Spawned prefab had no Monster component.");
        }
    }
}
