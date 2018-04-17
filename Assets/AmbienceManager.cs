using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceManager : MonoBehaviour {
    
    public Material Skybox1, Skybox2;
    public ParticleSystem pSystem1, pSystem2;
    
    public bool isSkybox1 = true;

    public void Start() {
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
            isSkybox1 = false;
        }else {
            RenderSettings.skybox = Skybox1;
            pSystem2.Stop();
            pSystem1.Play();
            isSkybox1 = true;
        }
    }    
}
