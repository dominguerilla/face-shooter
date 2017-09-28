using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// The common interface that is used by SpawnPoints to spawn and start the behaviors of monsters.
/// </summary>
public abstract class Monster : MonoBehaviour, IFlashable, IShootable {

    [SerializeField] protected int Health = 1;
    [SerializeField] protected Transform navTarget;         // keeps track of the target; that way, can resume behavior even after being stunned.
    [SerializeField] protected float attackDistance = 2.0f;
    [SerializeField] protected float timeBetweenAttacks = 1.0f;

    protected Wave wave;
    protected Animator anim;
    protected NavMeshAgent agent;

    protected virtual void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackDistance;
    }

    // For spawning the monster outside of a wave
    public virtual void StartBehavior()
    {

    }

    // For spawning the monster within the wave
    // this way, when the monster dies, it can notify the wave.
    public virtual void StartBehavior(Wave wave)
    {

    }

    /// <summary>
    /// Sets the navigation target to the given transform.
    /// Meant to be called before StartBehavior().
    /// </summary>
    /// <param name="target"></param>
    public virtual void SetTarget(Transform target)
    {
        navTarget = target;
    }

    /// <summary>
    /// Returns true only when within stopping distance of the target.
    /// </summary>
    protected virtual IEnumerator MoveTo(Transform target)
    {
        anim.SetBool("isMoving", true);
        if(agent.enabled)
            agent.SetDestination(target.transform.position);
        while(agent.enabled && agent.remainingDistance >= attackDistance)
        {
            yield return false;
        }
        anim.SetBool("isMoving", false);
        yield return true;
    }

    /// <summary>
    /// Returns true only when the attack animation is finished.
    /// </summary>
    protected virtual IEnumerator Attack(Transform target)
    {
        this.transform.LookAt(target);
        anim.SetTrigger("attack");
        yield return new WaitForSeconds(timeBetweenAttacks);
        yield return true;
    }

    // though death is similar, the specifics are what makes us unique
    protected virtual void Die()
    {
        if (wave != null)
            wave.livingMonsters.Remove(this);
    }

    #region INTERFACE IMPLEMENTATIONS
    public virtual void OnFire()
    {
        Debug.Log(gameObject.name + " has been shot!");
    }

    public virtual void OnFlash()
    {
        Debug.Log(gameObject.name + " has been hit by your flash!");
    }
    #endregion

    #region STUN RELATED FUNCTIONS

    protected IEnumerator Stunned(float stunTime)
    {
        EnableStun();
        yield return new WaitForSeconds(stunTime);
        DisableStun();
    }

    protected virtual void EnableStun()
    {
        agent.enabled = false;
        anim.speed = 0;
    }

    protected virtual void DisableStun()
    {
        anim.speed = 1;
        agent.enabled = true;
        if (navTarget)
            agent.SetDestination(navTarget.position);
    }
    #endregion
}
