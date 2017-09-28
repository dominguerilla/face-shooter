using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedSlime : Slime {

    public GameObject explosionPrefab;

    protected override void Start()
    {
        base.Start();
    }


    public override void OnFire()
    {
        Debug.Log(gameObject.name + " damaged!");
        this.Health--;

        // the only difference between this and Slime.OnFire() is that this spawns the explosionPrefab on death.
        if(Health <= 0)
        {
            Die();
        }
    }

    protected override void Die()
    {
        if (mainBehavior != null)
            StopCoroutine(mainBehavior);
        if (ambientSoundMaker != null)
            StopCoroutine(ambientSoundMaker);
        agent.enabled = false;
        anim.SetBool("isDead", true);
        Instantiate(explosionPrefab, this.transform.position, this.transform.rotation); // spawn explosion!
        GetComponent<Collider>().enabled = false;
        Destroy(this.gameObject, 0.5f);
    }
}
