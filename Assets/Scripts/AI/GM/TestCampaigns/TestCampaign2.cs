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
    void Start()
    {
        Wave.MonsterPopulation pop1 = new Wave.MonsterPopulation();
        Wave.MonsterPopulation pop2 = new Wave.MonsterPopulation();
        Wave.MonsterPopulation pop3 = new Wave.MonsterPopulation();

        Wave wave1 = new Wave();
        Wave wave2 = new Wave();
        Wave wave3 = new Wave();

        wave1.spawnNextWhenAllDie = true;
        wave2.spawnNextWhenAllDie = true;
        wave3.spawnNextWhenAllDie = true;

        pop1.type = monster;
        pop2.type = monster;
        pop3.type = monster;

        pop1.min = 1;
        pop1.max = 1;
        pop2.min = 2;
        pop2.max = 2;
        pop3.min = 3;
        pop3.max = 3;

        wave1.MonsterPopulations.Add(1, pop1);
        wave2.MonsterPopulations.Add(1, pop2);
        wave3.MonsterPopulations.Add(1, pop3);

        wave1.timeTillNextWave = timeBetweenWaves;
        wave2.timeTillNextWave = timeBetweenWaves;
        wave3.timeTillNextWave = timeBetweenWaves;

        Wave[] waves = new Wave[3] { wave1, wave2, wave3 };


        this.Waves = waves;
    }
}
