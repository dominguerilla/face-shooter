using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Makes GameObject plane constantly move while maintaining 'eye contact' with target.
/// </summary>
public class MoveAndLook : MonoBehaviour {

    public GameObject target;
    public float speed = 1.0f;
    public float timeInOneDirection = 5.0f;

	// Use this for initialization
	void Start () {
        StartCoroutine(MoveBackAndForth());
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.LookAt(target.transform, Vector3.up);	
	}

    // goes right first, then left
    IEnumerator MoveBackAndForth()
    {
        float time = 0.0f;
        int toggle = 1;
        while (true)
        {
            transform.Translate(speed * Time.deltaTime * toggle, 0, 0);
            time += Time.deltaTime;
            if(time >= timeInOneDirection)
            {
                time = 0.0f;
                toggle *= -1;
            }
            yield return null;
        }
    }
}
