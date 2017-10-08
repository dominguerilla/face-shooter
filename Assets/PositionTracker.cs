using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Tracks changes in position of this object.
public class PositionTracker : MonoBehaviour {

    public float timeBetweenChecks = 1.0f;
    public Vector3 lastCheckedPosition;

    IEnumerator checkRoutine;

    private IEnumerator Start()
    {
        lastCheckedPosition = this.transform.position;
        yield return new WaitForEndOfFrame();
        checkRoutine = StartChecking();
        StartCoroutine(checkRoutine);
    }

    IEnumerator StartChecking()
    {
        while (true)
        {
            lastCheckedPosition = this.transform.position;
            yield return new WaitForSeconds(timeBetweenChecks);
        }
    }

    private void OnDestroy()
    {
        StopCoroutine(checkRoutine);
    }
}
