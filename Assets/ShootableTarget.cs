using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShootableTarget : MonoBehaviour{

    public UnityEvent OnShot;
    public UnityEvent OffShot;
    public ParticleSystem onParticles;
    public bool isToggleable;
    public bool startOnWake = false;

    bool wasShot = false;


    IEnumerator Start()
    {
        if (startOnWake)
        {
            // delay required so that everything is set up before OnFire is called
            yield return new WaitForSeconds(3.0f);
            OnFire();
        }
    }

    public void OnFire()
    {
        if (isToggleable)
        {
            if (!wasShot)
            {
                OnShot.Invoke();
                if(onParticles)
                    onParticles.Stop();
            }else
            {
                OffShot.Invoke();
                if(onParticles)
                    onParticles.Play();
            }
            wasShot = !wasShot;
        }else
        {
            OnShot.Invoke();
        }
    }
}
