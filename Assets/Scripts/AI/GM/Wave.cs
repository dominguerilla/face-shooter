using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Each Campaign is made of multiple Waves.
/// Each Wave assigns a population of monsters to each spawnpoint.
/// </summary>
public class Wave  {
    
    public class MonsterPopulation
    {
        public GameObject type;        // the monster prefab
        public int min;   // the minimum number of this monster
        public int max;   // the maximum number of this monster
    }

    public float timeBetweenMonsters = 1.0f;    // the amount of time to pass before spawning each monster
    public float timeTillNextWave = 3.0f;       // the amount of time to wait after the last wave spawns
    public bool spawnNextWhenAllDie = false;     // if true, will wait till all Monsters die before spawning the next wave.
    public List<Monster> livingMonsters;               // the monsters spawned from this wave

    // <a spawnpoint index and a monsterpopulation.>
    // The spawnpoint index is the array of SpawnPoints where the monsters will come from.
    // For instance, index 0 could mean a group of SpawnPoints that are in the north,
    // while index 1 could mean a group of SpawnPoints in the west.
    public Dictionary<int, MonsterPopulation> MonsterPopulations;

    public Wave()
    {
        MonsterPopulations = new Dictionary<int, MonsterPopulation>();
        livingMonsters = new List<Monster>();
    }
}
