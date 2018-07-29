using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Switches between a calming atmosphere (skybox, particles, music) to a fighting one.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AmbienceManager : MonoBehaviour {
    
    public Material calmSkybox, fightSkybox;
    public ParticleSystem calmParticles, fightParticles;
    public AudioClip calmMusic; 
    public AudioClip fightMusic;

    public bool isCalm = true;

    private AudioSource aSource;
    private bool DEBUG_MODE;

    public void Start() {
        DEBUG_MODE = GameMaster.instance.DEBUG_MODE;
        aSource = GetComponent<AudioSource>();

        if(isCalm)
            StartCalm();
        else
            StartFight();


        if(!calmSkybox || !fightSkybox || !calmParticles || !fightParticles) {
            Debug.LogError("Skyboxes and particle systems not set!");
            Destroy(this);
        }
    }

    public void ToggleAmbience() {
        if(isCalm) {
            StartFight();
            isCalm = false;
        }else {
            StartCalm();
            isCalm = true;
        }
    }   
   
    private void StartCalm() {
        RenderSettings.skybox = calmSkybox;
        fightParticles.Stop();
        calmParticles.Play();

        aSource.clip = calmMusic;
        aSource.loop = true;
        if(!DEBUG_MODE)
            aSource.Play();
    }

    private void StartFight() {
        RenderSettings.skybox = fightSkybox;
        calmParticles.Stop();
        fightParticles.Play();

        aSource.clip = fightMusic;
        aSource.loop = false;
        if(!DEBUG_MODE)
            aSource.Play();
    }
}
