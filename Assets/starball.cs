using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Valve.VR.InteractionSystem.Throwable))]
public class starball : MonoBehaviour {

    public float spawnLife = 10.0f; // how long this starball will last if not picked up
    public float thrownLife = 30.0f; // how long this starball will last after being thrown
    public float glowLightMultiplier = 2.0f; // how much brighter this star glows after being picked up

    Coroutine despawnRoutine;
    Light glowLight;
    StarSpawner spawner; // used to deregister this starball when destroyed

    private void Start()
    {
        glowLight = GetComponent<Light>();
    }
    
    // The following two methods are separated so that they can be invoked by Unity's Event system
    public void StartInitialDespawn()
    {
        despawnRoutine = StartCoroutine(Despawn(spawnLife));
    }

    public void StartThrownDespawn()
    {
        despawnRoutine = StartCoroutine(Despawn(thrownLife));
    }

    public void StopDespawn()
    {
        StopCoroutine(despawnRoutine);
    }

    public void SetSpawner(StarSpawner spawner)
    {
        this.spawner = spawner;
    }

    public void IncreaseGlow()
    {
        glowLight.intensity *= glowLightMultiplier;
    }

    IEnumerator Despawn(float time)
    {
        yield return new WaitForSeconds(time);
        spawner.DeregisterStar(this);
        Destroy(this.gameObject);
    }
}
