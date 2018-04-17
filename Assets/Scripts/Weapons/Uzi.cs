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

    protected override IEnumerator Equipping()
    {
        isBeingPickedUp = true;
        while (true)
        {
            if (currentHand != null && currentHand.controller != null && currentHand.controller.GetPressUp(ActivateSkillButton))
            {
                isBeingPickedUp = false;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

}
