using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Uzi : Gun {

    public override string attachmentPoint
    {
        get { return "item_uzi_attachpoint"; }
        set { }
    }

    Animation anim;

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animation>();
        anim["Shoot"].wrapMode = WrapMode.Once; 
    }

    // this one is automatic fire!
    protected override void HandAttachedUpdate(Hand hand)
    {
        CheckToDetach(hand);

        if (equipped && hand.controller != null)
        {
            if (hand.controller.GetPress(ActivateSkillButton) && !isBeingPickedUp) //GetPress instead of GetPressDown
            {
                anim.Play();
                OnSkillButton.Invoke();
            }
        }
    }

    // TODO: make it so that this doesn't fire while you're equipping it
    protected override IEnumerator Equipping()
    {
        isBeingPickedUp = true;
        yield return new WaitForEndOfFrame();
        isBeingPickedUp = false;
    }
}
