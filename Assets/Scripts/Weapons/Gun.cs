using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

/// <summary>
/// Meant to be launched by a GunSpawner. Destroys itself if not picked up after being launched.
/// </summary>
public class Gun : EquippableSkillItem {

    public override ulong ActivateSkillButton
    {
        get
        {
            return SteamVR_Controller.ButtonMask.Trigger;
        }
    }
    
    public AudioClip[] GunshotSounds;
    public Transform firingPoint;       // Where the raycast for bullet collision will be fired from.
    public ParticleSystem muzzleFlash;  // The muzzle flash particle system.
    public float VibrationLength = 0.25f;
    [Range(0, 1)] public float VibrationStrength; // Strength of vibration when firing.

    public float damage = 1.0f;
    public int bullets = 6;       // the number of times you can shoot before the gun drops. Make it -1 for infinite bullets!
    public float cooldownTime = 2.0f;   // The length of time between successive shots, in seconds.
    public float range = 20.0f;
    public float bulletForce = 1.0f;
    public bool autoDespawn = true; // drops and despawns gun automatically when out of bullets/not picked up
    public float timeTillDespawn = 3.0f;
    public FaceEnemy.COLOR affinity = FaceEnemy.COLOR.NONE;

    protected GunSpawner spawner;  // the spawner that spawned this gun
    protected AudioSource audioSource;
    protected bool isFiring;  // true when gun is currently firing; this length of time is cooldownTime
    protected bool wasDropped; // true when the gun is dropped 
    protected bool wasEverEquipped = false;
    protected Light glow;
    protected FaceEnemy.DamageInformation damageInfo;

    public virtual void Fire()
    {
        if (!isFiring)
        {
            bullets--;
            StartCoroutine(Shoot());

            int randIndex = Random.Range(0, GunshotSounds.Length - 1);
            audioSource.clip = GunshotSounds[randIndex];
            audioSource.Play();
            muzzleFlash.Play();
            StartCoroutine(Vibration());

            RaycastHit hit;
            if (Physics.Raycast(firingPoint.position, firingPoint.forward, out hit, range))
            {
                IShootable shootable = hit.collider.GetComponent(typeof(IShootable)) as IShootable;
                if (shootable != null)
                {
                    shootable.OnFire(damageInfo);
                }

                if (hit.rigidbody)
                {
                    hit.rigidbody.AddForceAtPosition(bulletForce * firingPoint.forward, hit.point);
                }
            }

            if (bullets == 0)
            {
                // out of bullets!
                // click noise!
                // drop gun, do not allow it to be picked up, destroy gun
                if (currentHand.controller != null)
                {
                    Drop(currentHand);
                }
            }
        }
    }
    public void SetSpawner(GunSpawner spawner)
    {
        this.spawner = spawner;
    }

    protected override void Start()
    {
        base.Start();
        damageInfo = new FaceEnemy.DamageInformation();
        damageInfo.affinity = affinity;
        damageInfo.damage = damage;
        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
        {
            Debug.LogError("No Audio Source component found in " + gameObject.name + "!");
        }

        if (autoDespawn)
            StartCoroutine(StartAutoDespawn(timeTillDespawn));

        SetAffinity(affinity);
    }

    public virtual void SetAffinity(FaceEnemy.COLOR color)
    {
        affinity = color;
        Color lightColor;
        switch (color)
        {
            case FaceEnemy.COLOR.BLUE:
                lightColor = Color.blue;
                break;
            case FaceEnemy.COLOR.RED:
                lightColor = Color.red;
                break;
            default:
                return;
        }
        if(!glow)
            glow = gameObject.AddComponent<Light>();
        glow.color = lightColor;
        glow.range = 0.5f;
        glow.intensity *= 3;
    }

    protected virtual IEnumerator Shoot()
    {
        isFiring = true;
        yield return new WaitForSeconds(cooldownTime);
        isFiring = false;
    }

    // https://steamcommunity.com/app/358720/discussions/0/405693392914144440/
    protected virtual IEnumerator Vibration()
    {
        for (float i = 0; i < VibrationLength; i += Time.deltaTime)
        {
            if (currentHand == null || currentHand.controller == null)
                yield break;
            currentHand.controller.TriggerHapticPulse((ushort)Mathf.Lerp(0, 3999, VibrationStrength));
            yield return null;
        }
    }

    protected override void OnHandHoverBegin(Hand hand)
    {
        if (!equipped && !wasDropped)
        {
            if (hand.controller != null)
            {
                if (hand.controller.GetPress(PickUpButton))
                {
                    StartCoroutine(Equipping());
                    hand.AttachObject(gameObject, attachmentFlags, attachmentPoint);
                }
            }
        }
    }

    protected override void HandHoverUpdate(Hand hand)
    {
        if (!wasDropped && hand.controller != null)
        {
            if (hand.controller.GetPressDown(PickUpButton))
            {
                StartCoroutine(Equipping());
                hand.AttachObject(gameObject, attachmentFlags, attachmentPoint);
            }
        }
    }

    protected override void CheckToDetach(Hand hand)
    {
        if (equipped && hand.controller != null)
        {
            if (hand.controller.GetPressUp(DropButton))
            {
                Drop(hand);
            }
        }
    }

    protected override void OnAttachedToHand(Hand hand)
    {
        base.OnAttachedToHand(hand);
        wasEverEquipped = true;
    }

    protected IEnumerator StartAutoDespawn(float despawnTime)
    {
        yield return new WaitForSeconds(despawnTime);
        if(!wasEverEquipped)
            Drop(currentHand, 0.0f);
    }

    // Disallows equipping of this gun, as well as detaches and destroys it.
    protected virtual void Drop(Hand hand, float destroyTime = 1.5f)
    {
        wasDropped = true;
        if (currentHand)
        {
            currentHand.DetachObject(gameObject, false);
        }
        if (spawner)
            spawner.DeregisterGun(this);
        Destroy(gameObject, destroyTime);
    }

    protected void OnDestroy()
    {
        // not sure if this is necessary, but just in case.
        StopAllCoroutines();
    }
}

