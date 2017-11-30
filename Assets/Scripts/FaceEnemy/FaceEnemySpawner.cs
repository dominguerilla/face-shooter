using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns a wave of enemies. Requires a Campaign to be in the scene!
/// </summary>
public class FaceEnemySpawner : MonoBehaviour {
    
    public GameObject FaceEnemyPrefab;
    public GameObject target;
    public Campaign campaign;

    List<FaceEnemy> spawnedEnemies; //when enemies are spawned, all are added to this list. if destroyed, the entry will be null.
    
	// Use this for initialization
	void Start () {
        spawnedEnemies = new List<FaceEnemy>();
        if (!campaign)
        {
            campaign = GameObject.FindObjectOfType<Campaign>();
            if (!campaign)
            {
                Debug.LogError("No Campaign found in scene!");
            }
        }
    }

    public void SpawnFaces(Wave currentWave)
    {
        while (spawnedEnemies.Count < currentWave.numberOfEnemies)
        {
            Vector3 position = campaign.GetNextSpawnPosition(currentWave, this.transform.position);
            FaceEnemy enemy = CreateEnemy(position);
            campaign.InitializeEnemy(currentWave, enemy);
        }
        StartCoroutine(ActivateEnemies(currentWave.activeDelayTime));
    }

    public List<FaceEnemy> getSpawnedEnemies()
    {
        return spawnedEnemies;
    }

    public void ResetSpawner()
    {
        spawnedEnemies = new List<FaceEnemy>();
    }

    FaceEnemy CreateEnemy(Vector3 position)
    {
        GameObject obj = GameObject.Instantiate(FaceEnemyPrefab, position, this.transform.rotation);
        FaceEnemy enemy = obj.GetComponent<FaceEnemy>();
        spawnedEnemies.Add(enemy);
        return enemy;
    }

    IEnumerator ActivateEnemies(float activeDelayTime)
    {
        yield return new WaitForSeconds(activeDelayTime);
        foreach(FaceEnemy enemy in spawnedEnemies)
        {
            enemy.gameObject.SetActive(true);
            enemy.Activate();
        }
    }
}
