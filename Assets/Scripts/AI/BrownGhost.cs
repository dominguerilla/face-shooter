using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class BrownGhost : Monster {
    
    public float stunTime = 1.0f;

    public Material DefaultMat;
    public Material OnStunMat;
    public ParticleSystem OnDeathParticles;
    public bool StartBehaviorOnWake;

    bool isStunned;
    Renderer render;
    bool behaviorStarted = false;
    IEnumerator mainBehavior;

	protected override void Start () {
        base.Start();
        render = GetComponentInChildren<SkinnedMeshRenderer>();
        if (!render)
        {
            Debug.LogError("No MeshRenderer found under " + gameObject.name + "!");
        }

        if (StartBehaviorOnWake)
        {
            StartBehavior();
        }
    }

    public override void StartBehavior()
    {
        if(!behaviorStarted && navTarget)
        {
            //Debug.Log("Behavior started: Moving to and attacking " + navTarget);
            behaviorStarted = true;
            mainBehavior = MoveToAndAttack(navTarget);
            StartCoroutine(mainBehavior);
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

    public override void OnFire()
    {
        // Update health
        this.Health--;

        // if during stun, don't animate damage
        if (!isStunned)
        {
            agent.isStopped = true;
            anim.SetTrigger("damage");
            agent.isStopped = false;
        }

        // if dead
        // NOTE THAT DEATH IS ACTIVATED ONLY IN THIS FUNCTION
        if (Health <= 0 && OnDeathParticles)
        {
            if (mainBehavior != null)
                StopCoroutine(mainBehavior);
            OnDeathParticles.Play();
            agent.enabled = false;
            anim.SetBool("isDead", true);
            Destroy(this.gameObject, 2.0f);
            GetComponent<Collider>().enabled = false;
        }
    }


    #region STUN RELATED FUNCTIONS
    public override void OnFlash()
    {
        Debug.Log(gameObject.name + " has been stunned by your flash!");
        StartCoroutine(Stunned(stunTime));
    }

    protected override void EnableStun()
    {
        if (Health <= 0) return;
        render.material = OnStunMat;
        isStunned = true;
        if(agent.enabled)
            agent.isStopped = true;
        base.EnableStun();
    }

    protected override void DisableStun()
    {
        render.material = DefaultMat;
        isStunned = false;
        if(agent.enabled)
            agent.isStopped = false;
        base.DisableStun();
    }
    #endregion
}
