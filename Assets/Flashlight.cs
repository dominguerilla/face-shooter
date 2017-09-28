using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : EquippableSkillItem {

    public override ulong ActivateSkillButton
    {
        get
        {
            return SteamVR_Controller.ButtonMask.Trigger;
        }
    }

    public override string attachmentPoint
    {
        get { return "item_flashlight_attachpoint"; }
        set { }
    }

    Light lightSource;
    bool isOn;

    protected override void Start()
    {
        base.Start();
        lightSource = GetComponentInChildren<Light>();

        lightSource.enabled = false;
    }

    public void ToggleLight()
    {
        if (!isOn)
        {
            lightSource.enabled = true;
        }else
        {
            lightSource.enabled = false;
        }

        isOn = !isOn;
    }
}
