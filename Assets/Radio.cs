using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Radio : MonoBehaviour {

    AudioSource source;

    [Header("Wave Announcements")]
    public AudioClip[] waveAnnouncements;

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
        //GameMaster.instance.OnStartWave.AddListener(AnnounceNextWave);
	}

    public void AnnounceNextWave()
    {
        int index = Random.Range(0, waveAnnouncements.Length - 1);
        source.clip = waveAnnouncements[index];
        source.Play();
    }
}
