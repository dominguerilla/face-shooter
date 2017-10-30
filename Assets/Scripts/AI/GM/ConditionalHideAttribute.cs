using UnityEngine;
using System;
using System.Collections;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
    AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
// http://www.brechtos.com/hiding-or-disabling-inspector-properties-using-propertydrawers-within-unity-5/
public class ConditionalHideAttribute : PropertyAttribute
{
    //The name of the bool field that will be in control
    public string ConditionalSourceField = "";
    //TRUE = Hide in inspector / FALSE = Disable in inspector 
    public bool HideInInspector = false;

    // custom
    public int desiredEnumVal = -1;

    public ConditionalHideAttribute(string conditionalSourceField)
    {
        this.ConditionalSourceField = conditionalSourceField;
        this.HideInInspector = false;
    }

    public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector)
    {
        this.ConditionalSourceField = conditionalSourceField;
        this.HideInInspector = hideInInspector;
    }


    // These next two are also custom
    public ConditionalHideAttribute(string conditionalSourceField, int desiredEnumVal)
    {
        this.ConditionalSourceField = conditionalSourceField;
        this.HideInInspector = false;
        this.desiredEnumVal = desiredEnumVal;
    }

    public bool compareEnumValues(int actualEnumVal)
    {
        return desiredEnumVal == actualEnumVal;
    }
}