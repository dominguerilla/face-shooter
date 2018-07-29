using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

public class Switch : MonoBehaviour {

    public bool activateOnStart = false;
    public UnityEvent onActivate;

    LinearMapping lmap;
    bool activated = false;
	// Use this for initialization

	IEnumerator Start () {
        lmap = GetComponent<LinearMapping>();
        if (activateOnStart)
        {
            yield return new WaitForEndOfFrame();
            Activate();
        }
	}
	
	// Update is called once per frame
	void Update () {
	    if(!activated && lmap.value >= 0.9f)
        {
            Activate();
        }
	}

    void Activate()
    {
        onActivate.Invoke();
        activated = true;
    }
}
