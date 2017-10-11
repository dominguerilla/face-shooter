using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A collection of Wave specifications to spawn Monsters in a certain way.
/// Attach the Campaign object to the same object that the GameMaster component is attached to.
/// Ideally, campaign designers should only have to inherit this!
/// </summary>
[DisallowMultipleComponent]
public class Campaign : MonoBehaviour{

    public Wave[] Waves;             // the GM will spawn each Wave according to the specification

}
