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
                    //GM.Init();
                }
            }
            return GM;
        }
    }

    //public UnityEvent OnCampaignStart;
    //public UnityEvent OnStartWave;
    //public UnityEvent OnEndWave;

    public Campaign campaign;
    public FaceEnemySpawner spawner;

    public void StartCampaign()
    {
        StartCoroutine(CampaignWaves());
    }

    IEnumerator CampaignWaves()
    {
        foreach(Wave wave in campaign.Waves)
        {
            spawner.SetAttributes(wave);
            print("Wave number of enemies: " + wave.numberOfEnemies);
            print("Spawner number of enemies: " + spawner.numberOfEnemies);
            spawner.SpawnFaces();
            print("Spawning wave.");
            // wait till all enemies are killed
            List<FaceEnemy> enemies = spawner.getSpawnedEnemies();
            print("Found " + enemies.Count + " in this wave.");
            while (true)
            {
                foreach(FaceEnemy enemy in enemies)
                {
                    if(enemy == null)
                    {
                        enemies.Remove(enemy);
                        print(enemies.Count + " left.");
                        break;
                    }
                }
                if(enemies.Count == 0)
                {
                    break;
                }
                yield return new WaitForEndOfFrame();
            }
            print("Wave ended.");
            spawner.ResetSpawner();
        }
    }

}
