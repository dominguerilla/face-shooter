using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FaceEnemy : MonoBehaviour, IShootable {


    public struct DamageInformation
    {
        public COLOR affinity;
        public float damage;
        public RaycastHit hit;
    }

    public enum COLOR
    {
        NONE,
        RED,
        BLUE
    }

    public GameObject target;
    public float health = 1.0f;
    public COLOR affinity = COLOR.NONE;
    public float timeAsleep = 11.0f;
    public float timeAwake = 2.0f;
    public float chargeSpeed = 10.0f;
    public float stoppingDistance = 0.5f;
    public float damageDuration = 1.0f;

    // This enemy will spawn from the ground (spawningMat), stay still for a few seconds (awakeMat), and then charge at the target (chargingMat)
    // These materials are what differentiates the different phases.
    public Material[] spawningMats;
    public Material[] awakeMats;
    public Material[] chargingMats;
    [HideInInspector]
    public ParticleSystem onSpawnParticles = null; 
    [HideInInspector]
    public ParticleSystem onHitParticles; 
    [HideInInspector]
    public ParticleSystem onDeathParticles; 

    // the following particles need to already exist as a child of the FaceEnemy prefab
    // the indices of each is as follows:
    // 0: Particles on spawn
    // 1: Particles on hit
    // 2: Particles on death
    public ParticleSystem[] redParticles;
    public ParticleSystem[] blueParticles;

    [HideInInspector]
    public bool keepFacingPlayer = true;

    IEnumerator behavior;
    [HideInInspector]
    public int bouncesRemaining = 0; // used to remember state of number of bounces
    Renderer facePlaneRender;
    Light glow;
    bool stunned = false;
    float deathTime = 0.5f; // the amount of time it'll take for the gameobject to be destroyed after death.
    Collider hitCollider;
    bool isDying = false;

    // Use this for initialization
    void Start () {
        facePlaneRender = GetComponentInChildren<Renderer>();
        hitCollider = GetComponent<Collider>();
        int index = UnityEngine.Random.Range(0, spawningMats.Length);
        SwitchMaterial(spawningMats[index]);

        SetAffinity(affinity);
        var main = onDeathParticles.main;
        main.simulationSpeed = 3.0f;
        if(onSpawnParticles)
            onSpawnParticles.Play();
    }
	
	// Update is called once per frame
	void Update () {
        if(target && keepFacingPlayer)
            this.transform.LookAt(target.transform, Vector3.up);
    }
  
    // public so that the campaign can set affinity 
    public void SetAffinity(COLOR color)
    {
        Color lightColor;
        switch (color)
        {
            case COLOR.BLUE:
                lightColor = Color.blue;
                onSpawnParticles = blueParticles[0];
                onHitParticles = blueParticles[1];
                onDeathParticles = blueParticles[2];
                break;
            case COLOR.RED:
                lightColor = Color.red;
                onSpawnParticles = redParticles[0];
                onHitParticles = redParticles[1];
                onDeathParticles = redParticles[2];
                break;
            default: // defaults to showing red particles
                lightColor = Color.white;
                onSpawnParticles = redParticles[0];
                onHitParticles = redParticles[1];
                onDeathParticles = redParticles[2];
                break;
        }

        glow = gameObject.AddComponent<Light>();
        glow.color = lightColor;
        glow.range = 3.0f;
        glow.intensity *= 3;
    } 

    public void StartBehavior()
    {
        StartCoroutine(behavior);    
    }

    public void StopBehavior()
    {
        StopCoroutine(behavior);
    }

    public void SetBehavior(IEnumerator behavior)
    {
        this.behavior = behavior;
    }

    public void SwitchMaterial(Material newMat)
    {
        facePlaneRender.material = newMat;
    }

    public Material GetCurrentFace()
    {
        return facePlaneRender.material;
    }

    // reduced damage if the affinity does not match
    public void OnFire(DamageInformation info)
    {
        //play the onhit particle animation
        if (onHitParticles)
        {
            Vector3 hitLocation = info.hit.point;
            GameObject hitParticleObj = GameObject.Instantiate<GameObject>(onHitParticles.gameObject);
            ParticleSystem hitParticleInstance = hitParticleObj.GetComponent<ParticleSystem>();
            hitParticleInstance.transform.position = hitLocation;
            hitParticleInstance.Play();
        }

        float damage = info.damage;
        if(info.affinity != FaceEnemy.COLOR.NONE && info.affinity != affinity)
        {
            damage = damage / 2.0f;
        }

        health -= damage;
        if(health <= 0)
        {
            Die();
        }else if (!stunned)
        {
            // damage animation
            StartCoroutine(StartDamageAnimation());
        }
    }

    void Die()
    {
        if(!isDying) {
            isDying = true;
            facePlaneRender.enabled = false;
            hitCollider.enabled = false;
            onDeathParticles.Play();
            Destroy(this.gameObject, deathTime);
        }
    }

    IEnumerator StartDamageAnimation()
    {
        StopCoroutine(behavior);
        stunned = true;
        yield return FaceEnemyBehaviours.StartDamageAnimation(this, damageDuration);
        StartCoroutine(behavior);
        stunned = false;
        yield return null;
    }

    private void OnDestroy()
    {
         //StopCoroutine(behavior);
    }
}
