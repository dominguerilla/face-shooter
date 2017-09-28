using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraItem : EquippableSkillItem {


    public override string attachmentPoint
    {
        get { return "item_camera_attachpoint"; }
        set { }
    }

    public override ulong ActivateSkillButton
    {
        get
        {
            return SteamVR_Controller.ButtonMask.Trigger;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
