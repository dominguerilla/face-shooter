using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SHOOTS AT ANYTHING SHOOTABLE
/// </summary>
public class Turret : MonoBehaviour {

    public Transform firingPoint;
    public AudioSource firingSoundSource;
    public AudioClip[] GunshotSounds;

    public float cooldownTime = 2.0f;
    public ParticleSystem muzzleFlash;

    bool isFiring;
    bool justLostTarget;
    Transform neutralPosition;

    GameObject currentTarget;

    private void Start()
    {
        neutralPosition = transform;
    }

    private void Update()
    {
        if (currentTarget)
        {
            transform.LookAt(currentTarget.transform);
        }else if(justLostTarget)
        {
            transform.rotation = neutralPosition.rotation;
        }
    }

    public void Fire()
    {
        if (!isFiring)
        {
            StartCoroutine(Shoot());
            RaycastHit hit;

            int randIndex = Random.Range(0, GunshotSounds.Length - 1);
            firingSoundSource.clip = GunshotSounds[randIndex];
            firingSoundSource.Play();
            muzzleFlash.Play();

            if (Physics.Raycast(firingPoint.position, firingPoint.forward, out hit))
            {
                IShootable shootable = hit.collider.GetComponent(typeof(IShootable)) as IShootable;
                if (shootable != null)
                {
                    //shootable.OnFire();
                }
            }
        }
    }

    IEnumerator Shoot()
    {
        isFiring = true;
        yield return new WaitForSeconds(cooldownTime);
        isFiring = false;
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
