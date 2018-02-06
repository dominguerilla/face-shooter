using BehaviorDesigner.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FaceEnemy : MonoBehaviour, IShootable {


    public struct DamageInformation
    {
        public COLOR affinity;
        public float damage;
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

    
    ExternalBehaviorTree currentBehavior;
    BehaviorTree behavior;

    [HideInInspector]
    public bool keepFacingPlayer = true;

    Renderer facePlaneRender;
    Light glow;
    bool stunned = false;

    // Use this for initialization
    void Start () {
        facePlaneRender = GetComponentInChildren<Renderer>();
        int index = UnityEngine.Random.Range(0, spawningMats.Length);
        SwitchMaterial(spawningMats[index]);

        SetAffinity(affinity);
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
                break;
            case COLOR.RED:
                lightColor = Color.red;
                break;
            default:
                lightColor = Color.white;
                break;
        }

        glow = gameObject.AddComponent<Light>();
        glow.color = lightColor;
        glow.range = 3.0f;
        glow.intensity *= 3;
    } 

    public void StartBehavior()
    {
        behavior.Start();    
    }

    public void StopBehavior()
    {
        behavior.DisableBehavior();
    }

    public void SetBehavior(ExternalBehaviorTree behavior)
    {
        this.behavior.ExternalBehavior = behavior;
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
        float damage = info.damage;

        if(info.affinity != FaceEnemy.COLOR.NONE && info.affinity != affinity)
        {
            damage = damage / 2.0f;
        }

        health -= damage;
        if(health <= 0)
        {
            Destroy(this.gameObject);
        }else if (!stunned)
        {
            // damage animation
            StartCoroutine(StartDamageAnimation());
        }
    }

    IEnumerator StartDamageAnimation()
    {
        StopBehavior();
        stunned = true;
        yield return FaceEnemyBehaviours.StartDamageAnimation(this, damageDuration);
        StartBehavior();
        stunned = false;
        yield return null;
    }

}
