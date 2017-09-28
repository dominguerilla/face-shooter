using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Should only be one of these per level.
/// Finds the currently attached Campaign object and spawns Monsters according to its Wave specifications
/// Ideally, campaign designer should only have to specify the groups of spawn points that campaigns are able to use in each level.
/// 
/// TODO: Make this a Singleton pattern
/// </summary>
public class GameMaster : MonoBehaviour {

    private static GameMaster GM;
    public static GameMaster instance {
        get {
            if(!GM)
            {
                GM = FindObjectOfType(typeof(GameMaster)) as GameMaster;
                if (!GM)
                {
                    Debug.LogError("No GameMaster script present in scene.");
                }else
                {
                    GM.Init();
                }
            }
            return GM;
        }
    }

    public UnityEvent OnCampaignStart;
    public UnityEvent OnStartWave;
    public UnityEvent OnEndWave;

    /// <summary>
    /// The object of the GameMaster's affection.
    /// </summary>
    public Transform target;

    /// <summary>
    /// Assign in Inspector! Each entry should be a GameObject with SpawnPoint prefab(s) as children.
    /// </summary>
    /// <notes>So, if there are three general areas where enemies come from, there should be three entries.</notes>
    public GameObject[] spawnAreas;

    private SpawnPoint[][] availableSpawnPoints;
    private List<Monster> currentlySpawnedMonsters;

    Campaign campaign;

    private void Init()
    {
        currentlySpawnedMonsters = new List<Monster>();

        // set up campaign
        campaign = GM.GetComponent(typeof(Campaign)) as Campaign;
        if (!campaign)
        {
            Debug.LogError("No Campaign type component found on GM object!");
            Destroy(this);
        }

        // get the spawnpoints
        availableSpawnPoints = new SpawnPoint[spawnAreas.Length][];
        for(int i = 0; i < spawnAreas.Length; i++)
        {
            availableSpawnPoints[i] = spawnAreas[i].GetComponentsInChildren<SpawnPoint>();
            if(availableSpawnPoints[i].Length == 0)
            {
                Debug.LogError("Spawn Area " + spawnAreas[i].name + " contains no SpawnPoint children!");
            }
        }

        // set up UnityEvents
        OnCampaignStart = new UnityEvent();
        OnStartWave = new UnityEvent();
        OnEndWave = new UnityEvent();
    }

    public void StartCampaign()
    {
        GM.OnCampaignStart.Invoke();
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        foreach(Wave wave in GM.campaign.Waves)
        {
            GM.OnStartWave.Invoke();
            yield return SpawnWave(wave);

            // will not spawn the next wave until all of the current monsters have been killed if the 
            // wave specfies it
            // TODO -- find a way to remove this monster once it's been killed
            // maybe like a listener on this script?
            if (wave.spawnNextWhenAllDie)
            {
                while(wave.livingMonsters.Count > 0)
                {
                    yield return new WaitForSeconds(5.0f);
                }
            }

            GM.OnEndWave.Invoke();
            yield return new WaitForSeconds(wave.timeTillNextWave);
        }
    }
    
    IEnumerator SpawnWave(Wave wave)
    {

        foreach(KeyValuePair<int, Wave.MonsterPopulation> pair in wave.MonsterPopulations)
        {
            int areaIndex = pair.Key;
            Wave.MonsterPopulation population = pair.Value;
            int totalPopulation = Random.Range(population.min, population.max);

            for(int i = 0; i < totalPopulation; i++)
            {
                SpawnPoint[] spawnPoints = GM.availableSpawnPoints[areaIndex];
                int pointIndex = Random.Range(0, spawnPoints.Length - 1);
                SpawnPoint point = spawnPoints[pointIndex];

                // spawn monster and keep track of it
                GameObject monsObj = Instantiate(pair.Value.type, point.transform.position, point.transform.rotation) as GameObject;
                Monster mons = monsObj.GetComponent(typeof(Monster)) as Monster;
                mons.SetTarget(target);
                mons.StartBehavior();
                wave.livingMonsters.Add(mons);  // the Monster shall remove itself from this list

                Debug.Log("GM spawned " + monsObj.name + " at " + point.gameObject.name);

                yield return new WaitForSeconds(wave.timeBetweenMonsters);
            }
        }
    }

}
