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
    FaceEnemy[] enemies;
    bool isActivated = false;
    
    private void Awake()
    {
        onPlay = new UnityEvent();
    }

    // Use this for initialization
    void Start () {
        enemies = GameObject.FindObjectsOfType<FaceEnemy>();
        audioSrc = GetComponent<AudioSource>();

        if (activateOnStart)
            StartCoroutine(activateHorde(activateDelay));
	}

    public void OnFire()
    {
        StartCoroutine(activateHorde(activateDelay));
    }

    IEnumerator activateHorde(float delay)
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
