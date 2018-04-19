using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AmbienceManager : MonoBehaviour {
    
    public Material Skybox1, Skybox2;
    public ParticleSystem pSystem1, pSystem2;
    public AudioClip music; // the ambient music to loop

    public bool isSkybox1 = true;

    private AudioSource aSource;

    public void Start() {
        aSource = GetComponent<AudioSource>();
        aSource.clip = music;
        aSource.loop = true;
        aSource.Play();

        if(!Skybox1 || !Skybox2 || !pSystem1 || !pSystem2) {
            Debug.LogError("Skyboxes and particle systems not set!");
            Destroy(this);
        }

        if(isSkybox1) {
            RenderSettings.skybox = Skybox1;
            pSystem1.Play();
        }else {
            RenderSettings.skybox = Skybox2;
            pSystem2.Play();
        }
    }

    public void ToggleSkyboxes() {
        if(isSkybox1) {
            RenderSettings.skybox = Skybox2;
            pSystem1.Stop();
            pSystem2.Play();
            StopMusic();
            isSkybox1 = false;
        }else {
            RenderSettings.skybox = Skybox1;
            pSystem2.Stop();
            pSystem1.Play();
            StartMusic();
            isSkybox1 = true;
        }
    }    

    public void StartMusic() {
        if(aSource.clip)
            aSource.Play();
    }

    public void StopMusic() {
        aSource.Stop();
    }
}
