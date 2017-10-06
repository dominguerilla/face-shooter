using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Revolver : Gun {

    public override string attachmentPoint
    {
        get { return "item_revolver_attachpoint"; }
        set { }
    }


    // Use this for initialization
    protected override void Start () {
        base.Start();
	}

    
}
