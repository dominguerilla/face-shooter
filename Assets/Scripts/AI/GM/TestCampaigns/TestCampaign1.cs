using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns the first monster from the front,
/// the second monster from the right,
/// and the third monster from the left.
/// </summary>
public class TestCampaign1 : Campaign {

    [Header("Monster Specs")]
    public GameObject wave1Monster;
    public GameObject wave2Monster;
    public GameObject wave3Monster;

    public float timeBetweenWaves = 5.0f;

	// Use this for initialization
	void Start () {
        Wave.MonsterPopulation pop1 = new Wave.MonsterPopulation();
        Wave.MonsterPopulation pop2 = new Wave.MonsterPopulation();
        Wave.MonsterPopulation pop3 = new Wave.MonsterPopulation();

        Wave wave1 = new Wave();
        Wave wave2 = new Wave();
        Wave wave3 = new Wave();

        pop1.type = wave1Monster;
        pop2.type = wave2Monster;
        pop3.type = wave3Monster;

        pop1.min = 1;
        pop1.max = 1;
        pop2.min = 1;
        pop2.max = 1;
        pop3.min = 1;
        pop3.max = 1;

        wave1.MonsterPopulations.Add(0, pop1);
        wave2.MonsterPopulations.Add(1, pop2);
        wave3.MonsterPopulations.Add(2, pop3);

        wave1.timeTillNextWave = timeBetweenWaves;
        wave2.timeTillNextWave = timeBetweenWaves;
        wave3.timeTillNextWave = timeBetweenWaves;

        Wave[] waves = new Wave[3] { wave1, wave2, wave3 };
        

        this.Waves = waves;

    }
}
