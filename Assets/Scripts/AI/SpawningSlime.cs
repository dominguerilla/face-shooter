using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This monster goes towards the target, stops a certain distance, and then continuously spawns other monsters until killed.
/// </summary>
public class SpawningSlime : Slime {

    public float distanceToSpawn = 5.0f;    // the stopping distance of the slime
    public GameObject spawnling;            // the gameobject that's created when spawning
    public Transform spawnlingTarget;       // the target of the spawnlings
    public float timeBetweenSpawnlings = 5.0f;  // the time between individual spawnlings
    public float spawnDistanceFactor = 0.5f;

    protected override void Start()
    {
        base.Start();
        agent.stoppingDistance = distanceToSpawn;
    }

    public override void StartBehavior()
    {
        StartCoroutine(MoveToAndSpawnMonsters(navTarget, spawnlingTarget));
    }

    protected IEnumerator MoveToAndSpawnMonsters(Transform location, Transform target)
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        while(Health > 0)
        {
            if (Vector3.Distance(gameObject.transform.position, location.transform.position) > agent.stoppingDistance)
            {
                yield return StartCoroutine(MoveTo(location));
            }else
            {
                anim.SetTrigger("attack");
                yield return new WaitForSeconds(0.1f); // so that the monster spawns at the end of the animation
                SpawnMonster();
                yield return new WaitForSeconds(timeBetweenSpawnlings);
            }
        }        
    }

    protected void SpawnMonster()
    {
        GameObject spawned = Instantiate(spawnling, transform.position + (transform.forward * spawnDistanceFactor) , transform.rotation) as GameObject;
        Monster mons = spawned.GetComponent<Monster>();
        if (mons)
        {
            Debug.Log("Spawnling created.");
            mons.SetTarget(spawnlingTarget);
            mons.StartBehavior();
        }else
        {
            Debug.Log("Spawnling had no monster component.");
        }
    }

    public override void OnFlash()
    {
        Debug.Log(gameObject.name + " is unaffected by your flash!");
    }
}
