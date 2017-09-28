using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;
using Valve.VR.InteractionSystem;

/// <summary>
/// Generic script to handle items that can be picked up, and that activates something when pressing the ActivateSkillButton.
/// </summary>
[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(Rigidbody))]
public class EquippableSkillItem : MonoBehaviour {

    public UnityEvent OnSkillButton;
    public virtual string attachmentPoint { get { return ""; } set { } }
    public virtual ulong ActivateSkillButton { get { return SteamVR_Controller.ButtonMask.ApplicationMenu; } }

    public virtual Hand.AttachmentFlags attachmentFlags {
        get { return Hand.AttachmentFlags.ParentToHand | Hand.AttachmentFlags.DetachFromOtherHand | Hand.AttachmentFlags.SnapOnAttach; }
        set { }
    } 

    // Whether or not this item hoverlocks when picking up
    public bool HoverLock = true;
    protected Rigidbody rb;
    protected bool equipped = false;
    protected bool isBeingPickedUp = false; // true in the same frame that the item is being equipped.

    protected Hand currentHand;     // the hand currently holding this object. Null if not held.

    // Use this for initialization
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    
    protected virtual void HandHoverUpdate(Hand hand)
    {
        if (hand.controller != null)
        {
            if (hand.controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                Debug.Log("Attachment point: " + attachmentPoint);
                StartCoroutine(Equipping());
                hand.AttachObject(gameObject, attachmentFlags, attachmentPoint);
            }
        }
    }

    protected virtual void OnAttachedToHand(Hand hand)
    {
        currentHand = hand;
        if(HoverLock)
            hand.HoverLock(null);
        rb.isKinematic = true;
        equipped = true;
    }

    protected virtual void OnDetachedFromHand(Hand hand)
    {
        currentHand = null;
        if(HoverLock)
            hand.HoverUnlock(null);
        equipped = false;
        rb.isKinematic = false;
    }

    protected virtual void HandAttachedUpdate(Hand hand)
    {
        CheckToDetach( hand );

        if (equipped && hand.controller != null)
        {   
            if (hand.controller.GetPressDown(ActivateSkillButton) && !isBeingPickedUp)
            {
                OnSkillButton.Invoke();
            }
        }
    }

    protected virtual void CheckToDetach( Hand hand )
    {
        if(equipped && hand.controller != null)
        {
            if (hand.controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
            {
                hand.DetachObject(gameObject, false);
            }
        }
    }

    protected IEnumerator Equipping()
    {
        isBeingPickedUp = true;
        yield return new WaitForEndOfFrame();
        isBeingPickedUp = false;
    }

}
