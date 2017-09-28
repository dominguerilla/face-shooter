using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : EquippableSkillItem {

    public override ulong ActivateSkillButton
    {
        get
        {
            return SteamVR_Controller.ButtonMask.Trigger;
        }
    }

    public override string attachmentPoint
    {
        get { return "item_revolver_attachpoint"; }
        set { }
    }

    public AudioClip[] GunshotSounds;
    public float cooldownTime = 2.0f;   // The length of time between successive shots, in seconds.
    public bool isAuto = false;         // True if it keeps firing while the trigger is pulled.
    public float range = 20.0f;
    public Transform firingPoint;       // Where the raycast for bullet collision will be fired from.
    public ParticleSystem muzzleFlash;  // The muzzle flash particle system.
    public float VibrationLength = 0.25f;
    [Range(0, 1)] public float VibrationStrength; // Strength of vibration when firing.

    AudioSource audioSource;
    bool isFiring;

    protected override void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
        {
            Debug.LogError("No Audio Source component found in " + gameObject.name + "!");
        }

        
    }

    public void Fire()
    {
        if (!isFiring)
        {
            StartCoroutine(Shoot());
            RaycastHit hit;

            int randIndex = Random.Range(0, GunshotSounds.Length - 1);
            audioSource.clip = GunshotSounds[randIndex];
            audioSource.Play();
            muzzleFlash.Play();
            StartCoroutine(Vibration());

            if (Physics.Raycast(firingPoint.position, firingPoint.forward, out hit, range))
            {
                IShootable shootable = hit.collider.GetComponent(typeof(IShootable)) as IShootable;
                if (shootable != null)
                {
                    shootable.OnFire();
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

    // https://steamcommunity.com/app/358720/discussions/0/405693392914144440/
    IEnumerator Vibration()
    {
        for(float i = 0; i < VibrationLength; i += Time.deltaTime)
        {
            currentHand.controller.TriggerHapticPulse((ushort)Mathf.Lerp(0, 3999, VibrationStrength));
            yield return null;
        }
    }
}

