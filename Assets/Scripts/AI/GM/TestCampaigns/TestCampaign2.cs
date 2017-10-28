using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This should spawn 1, 2, then 3 monsters from the right.
/// Each wave should only appear after all of the monsters in the previous wave are killed.
/// </summary>
public class TestCampaign2 : Campaign
{

    [Header("Monster Specs")]
    public GameObject monster;

    public float timeBetweenWaves = 7.0f;

    // Use this for initialization

}
