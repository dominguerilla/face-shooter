using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Slime : Monster {

    bool behaviorStarted;

    [Header("Sound")]
    public float timeBetweenSounds = 4.0f;
    public AudioClip[] ambientSounds;
    public bool startBehaviorOnWake = false;

    protected IEnumerator mainBehavior, ambientSoundMaker;
    protected AudioSource audioSource;

    protected override void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();

        if (startBehaviorOnWake)
            StartBehavior();
    }

    public override void OnFlash()
    {
        // flash don't affect this monster
    }

    public override void StartBehavior()
    {
        if (!behaviorStarted && navTarget)
        {
            //Debug.Log("Behavior started: Moving to and attacking " + navTarget);
            behaviorStarted = true;
            mainBehavior = MoveToAndAttack(navTarget);
            ambientSoundMaker = MakeAmbientSounds();
            StartCoroutine(mainBehavior);
            StartCoroutine(ambientSoundMaker);
        }
    }

    protected IEnumerator MoveToAndAttack(Transform target)
    {
        // TODO: These next two lines are necessary because for whatever reason, these are null
        // if spawned by SpawnPoint(). 
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        while (Health > 0)
        {
            if (Vector3.Distance(gameObject.transform.position, target.transform.position) > agent.stoppingDistance)
                yield return StartCoroutine(MoveTo(target));
            else yield return StartCoroutine(Attack(target));
        }
    }

    protected IEnumerator MakeAmbientSounds()
    {
        if(ambientSounds.Length == 0)
        {
            yield return null;
        }else
        {
            audioSource = GetComponent<AudioSource>();
            float startDelay = Random.Range(1.0f, 3.0f);
            yield return new WaitForSeconds(startDelay);
            while (true)
            {
                int index = Random.Range(0, ambientSounds.Length - 1);
                audioSource.clip = ambientSounds[index];
                audioSource.Play();
                float delay = Random.Range(timeBetweenSounds, timeBetweenSounds + 3.0f);
                yield return new WaitForSeconds(delay);
            }
        }
    }

    public override void OnFire()
    {
        // Update health
        this.Health--;

        // if dead
        // NOTE THAT DEATH IS ACTIVATED ONLY IN THIS FUNCTION
        if (Health <= 0)
        {
            Die();
        }
    }

    protected override void Die()
    {
        if (mainBehavior != null)
            StopCoroutine(mainBehavior);
        if (ambientSoundMaker != null)
            StopCoroutine(ambientSoundMaker);
        agent.enabled = false;
        anim.SetBool("isDead", true);
        Destroy(this.gameObject, 2.0f);
        GetComponent<Collider>().enabled = false;
    }
}
