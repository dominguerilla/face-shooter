using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shoots at things using a Gun.
/// </summary>
public class Turret : MonoBehaviour {

    public Gun gun;

    bool isFiring;
    bool justLostTarget;
    Transform neutralPosition;

    public GameObject currentTarget;

    private void Awake()
    {
        if(!gun) {
            gun = GetComponentInChildren<Gun>();
            if(!gun) {
                Debug.LogError("No gun assigned to the turret!");
                Destroy(this);
            }
        }

        neutralPosition = transform;
        
        // Initialize gun properties
        gun.GetComponent<Rigidbody>().useGravity = false;
        gun.autoDespawn = false;
        gun.bullets = -1; // infinite ammo
        Collider[] colliders = gun.GetComponents<Collider>();
        foreach(Collider col in colliders) {
            col.enabled = false;
        }
        
    }

    
    private void Update()
    {
        if (currentTarget)
        {
            transform.LookAt(currentTarget.transform);
            Fire();
        }else if(justLostTarget)
        {
            transform.rotation = neutralPosition.rotation;
        }
    }

    public void Fire()
    {
        gun.Fire();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!currentTarget)
        {
            IShootable shootable = other.GetComponent(typeof(IShootable)) as IShootable;
            if (shootable != null)
            {
                currentTarget = other.gameObject;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!currentTarget)
        {
            IShootable shootable = other.GetComponent(typeof(IShootable)) as IShootable;
            if (shootable != null)
            {
                currentTarget = other.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentTarget)
        {
            currentTarget = null;
        }
        StartCoroutine(LostTarget());
    }

    IEnumerator LostTarget()
    {
        justLostTarget = true;
        yield return new WaitForEndOfFrame();
        justLostTarget = false;
    }
}
