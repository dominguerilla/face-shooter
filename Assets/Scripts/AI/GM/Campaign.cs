using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A collection of Wave specifications to spawn Monsters in a certain way.
/// Requires a FaceEnemySpawner component to be present in the scene.
/// 
/// Ideally, campaign designers should only have to inherit this!
/// </summary>
[DisallowMultipleComponent]
public abstract class Campaign : MonoBehaviour{

    public FaceEnemySpawner spawner;
    public Wave[] Waves;

    protected Queue<Vector3> spawnPositions;
    protected Positioner positioner;

    protected virtual void Start()
    {
        if (!spawner)
        {
            spawner = GameObject.FindObjectOfType<FaceEnemySpawner>();
            if (!spawner)
            {
                Debug.LogError("No FaceEnemySpawner found in scene!");
            }
        }

        positioner = new Positioner();
    }

    public virtual void InitializeEnemy(Wave currentWave, FaceEnemy enemy)
    {
        enemy.target = currentWave.target;
        enemy.timeAsleep = currentWave.timeAsleep;
        enemy.timeAwake = currentWave.timeAwake;
        enemy.attackSpeed = currentWave.attackSpeed;
        enemy.stoppingDistance = currentWave.stoppingDistance;
        enemy.SetBehavior(GetEnemyBehaviour(enemy, currentWave.SpawnBehaviour, currentWave));
    }

    public virtual Vector3 GetNextSpawnPosition(Wave currentWave, Vector3 spawnerPosition)
    {
        if (spawnPositions == null || spawnPositions.Count == 0)
            spawnPositions = positioner.GetPositions(currentWave, spawnerPosition);
            
        return spawnPositions.Dequeue();
    }
    
    public virtual void StartCampaign()
    {
        StartCoroutine(CampaignWaves());
    }

    protected virtual IEnumerator CampaignWaves()
    {
        foreach (Wave wave in Waves)
        {
            spawner.SpawnFaces(wave);

            // wait till all enemies are killed before spawning the next wave
            List<FaceEnemy> enemies = spawner.getSpawnedEnemies();
            while (true)
            {
                foreach (FaceEnemy enemy in enemies)
                {
                    if (enemy == null)
                    {
                        enemies.Remove(enemy);
                        break;
                    }
                }
                if (enemies.Count == 0)
                {
                    break;
                }
                yield return new WaitForEndOfFrame();
            }
            spawner.ResetSpawner();
        }
    }

    protected virtual IEnumerator GetEnemyBehaviour(FaceEnemy enemy, FaceEnemyBehaviours.SPAWN_BEHAVIOURS behaviour, Wave currentWave)
    {
        switch (behaviour)
        {
            case FaceEnemyBehaviours.SPAWN_BEHAVIOURS.WAKE_THEN_ATTACK:
                return FaceEnemyBehaviours.WakeThenAttack(enemy, enemy.target.transform);
            case FaceEnemyBehaviours.SPAWN_BEHAVIOURS.TRAVEL_BACK_AND_FORTH_THEN_ATTACK:
                return FaceEnemyBehaviours.TravelBackAndForthThenAttack(enemy, enemy.target.transform, currentWave.bounceRadius, currentWave.numberOfBounces);
            default:
                throw new System.NotImplementedException();
        }
    }
}
