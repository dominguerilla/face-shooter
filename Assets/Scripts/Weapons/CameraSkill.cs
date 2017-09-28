using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSkill : MonoBehaviour {

    public Light lightSource;
    // Duration of flash in millisecs
    public ulong flashDuration = 10;

    // Whether or not the camera is currently flashing
    public bool isFlashing { get; private set; }

    // The length of time between successive shots, in seconds.
    public float cooldownTime = 1.0f;

    private List<Collider> currentlyColliding;

    private void Start()
    {
        currentlyColliding = new List<Collider>();
        if (lightSource.enabled)
            lightSource.enabled = false;
    }
    
	public void Flash()
    {
        if (!isFlashing)
        {
            Debug.Log("Camera flash!");
            StartCoroutine(StartFlash());
            foreach (Collider col in currentlyColliding)
            {
                if (col == null)
                    continue;
                IFlashable flashable = col.GetComponent(typeof(IFlashable)) as IFlashable;
                if (flashable != null)
                {
                    flashable.OnFlash();
                }
            }
        }
    }

    IEnumerator StartFlash()
    {
        StartCoroutine(Cooldown());
        lightSource.enabled = true;
        yield return new WaitForSeconds(flashDuration * 0.001f);
        lightSource.enabled = false;
    }

    IEnumerator Cooldown()
    {
        isFlashing = true;
        yield return new WaitForSeconds(cooldownTime);
        isFlashing = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!currentlyColliding.Contains(other))
        {
            currentlyColliding.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (currentlyColliding.Contains(other))
        {
            currentlyColliding.Remove(other);
        }
    }
}
