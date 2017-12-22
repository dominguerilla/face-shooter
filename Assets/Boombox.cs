using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Boombox : MonoBehaviour, IShootable {

    public bool activateOnStart = false;
    public float activateDelay = 0.0f;
    public UnityEvent onPlay;
    public bool DEBUG_MODE = false;

    AudioSource audioSrc;
    bool isActivated = false;
    
    private void Awake()
    {
        //onPlay = new UnityEvent();
    }

    // Use this for initialization
    void Start () {
        audioSrc = GetComponent<AudioSource>();

        if (activateOnStart)
            StartCoroutine(PlayMusic(activateDelay));
	}

    public void StartMusic()
    {
        StartCoroutine(PlayMusic(activateDelay));
    }

    public void OnFire(FaceEnemy.DamageInformation info)
    {
        StartMusic();
    }

    IEnumerator PlayMusic(float delay)
    {
        if (isActivated)
        {
            yield break; 
        }

        yield return new WaitForSeconds(delay);
        onPlay.Invoke();
        if(!DEBUG_MODE)
            audioSrc.Play();
        yield return null;
    }
}
