using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Each Campaign is made of multiple Waves.
/// Each Wave object represents a state of the FaceEnemySpawner.
/// </summary>
[System.Serializable]
public class Wave {

    public GameObject FaceEnemyPrefab;
    public GameObject target;

    public Positioner.SPAWN_PATTERNS SpawnPattern;
    public int numberOfEnemies = 3;
    public float activeDelayTime;
    // Sphere spawning stuff
    public float minSpawnDistanceFromCenter = 1.0f;
    public float maxSpawnDistanceFromCenter = 10.0f;
    // Line spawning stuff
    public float maxLineLength = 10.0f;

    public FaceEnemyBehaviours.SPAWN_BEHAVIOURS SpawnBehaviour;
    public float attackSpeed = 10.0f;
    public float stoppingDistance = 0.5f;
    public float timeAsleep = 1.0f;
    public float timeAwake = 1.5f;

    // for bounce pattern
    public float bounceRadius = 10.0f;
    public int numberOfBounces = 2;
}
