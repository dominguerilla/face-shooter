using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FaceEnemy : MonoBehaviour, IShootable {

    public GameObject target;
    public float timeAsleep = 11.0f;
    public float timeAwake = 2.0f;
    public float attackSpeed = 10.0f;
    public float stoppingDistance = 0.5f;

    // This enemy will spawn from the ground (spawningMat), stay still for a few seconds (awakeMat), and then charge at the target (chargingMat)
    // These materials are what differentiates the different phases.
    public Material[] spawningMats;
    public Material[] awakeMats;
    public Material[] chargingMats;

    [HideInInspector]
    public bool keepFacingPlayer = true;

    IEnumerator behavior;
    Renderer facePlaneRender;

    // Use this for initialization
    void Start () {
        facePlaneRender = GetComponentInChildren<Renderer>();
        int index = UnityEngine.Random.Range(0, spawningMats.Length);
        SwitchMaterial(spawningMats[index]);
    }
	
	// Update is called once per frame
	void Update () {
        if(keepFacingPlayer)
            this.transform.LookAt(target.transform, Vector3.up);
    }
    
    public void Activate()
    {
        StartCoroutine(behavior);    
    }

    public void SetBehavior(IEnumerator behavior)
    {
        this.behavior = behavior;
    }
    
    public void SwitchMaterial(Material newMat)
    {
        facePlaneRender.material = newMat;
    }

    public void OnFire()
    {
        StopCoroutine(behavior);
        this.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        StopCoroutine(behavior);
    }
}
