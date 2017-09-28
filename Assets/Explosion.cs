using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When spawned, immediately explodes, damaging all Monsters in the collider's range.
/// To use, just spawn this prefab at the point of explosion.
/// </summary>
public class Explosion : MonoBehaviour {

    public bool debugLog = false;
    public ParticleSystem explosionParticle;
    public int Damage = 3;
    public Collider rangeCollider;                    // not wasting the GetComponent call

    List<Monster> allInRange;

    private void Awake()
    {
        allInRange = new List<Monster>();
    }

	// Use this for initialization
	IEnumerator Start () {
        
        if (!explosionParticle)
        {
            Debug.LogError("No explosion particle specified.");
        }
        rangeCollider.enabled = false;
        rangeCollider.enabled = true;

        yield return new WaitForEndOfFrame();
        Explode();
    }
	
    /// <summary>
    /// Damages all Monsters in vicinity.
    /// </summary>
	public void Explode()
    {
        // Monster.OnFire() decrements by 1 health, and is currently the only thing that can activate death.
        // Just to make it so that OnFire() is still called....
        int damage = Damage - 1;
        if (damage >= 0)
            damage = 1;

        explosionParticle.Play();
        foreach(Monster mons in allInRange)
        {
            if(debugLog)
                Debug.Log("Damaging" + mons.gameObject);
            mons.OnFire();
        }

        Destroy(this.gameObject, 2.0f);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Monster mons = other.GetComponent(typeof(Monster)) as Monster;
        if (mons && !allInRange.Contains(mons))
        {
            if(debugLog)
                Debug.Log("Added " + other.name + " to monster list.");
            allInRange.Add(mons);
        }
    }

    // Do I need to make an OnTriggerExit()?
}
