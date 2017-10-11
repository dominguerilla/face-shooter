using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class FaceEnemySpawner : MonoBehaviour {
    public enum SPAWN_PATTERNS
    {
        SPHERE,
        VERTICAL_LINE
    }
    public enum SPAWN_BEHAVIOURS
    {
        WAKE_THEN_ATTACK,
        TRAVEL_BACK_AND_FORTH_THEN_ATTACK
    }

    public GameObject FaceEnemyPrefab;
    public GameObject target;
    
    public SPAWN_PATTERNS SpawnPattern = SPAWN_PATTERNS.SPHERE;
    public int numberOfEnemies = 10;
    public float activeDelayTime = 12.0f;
    // Sphere spawning stuff
    public float minSpawnDistanceFromCenter = 1.0f;
    public float maxSpawnDistanceFromCenter = 10.0f;
    // Line spawning stuff
    public float maxLineLength = 10.0f;
    
    public SPAWN_BEHAVIOURS SpawnBehaviour = SPAWN_BEHAVIOURS.WAKE_THEN_ATTACK;
    public float attackSpeed = 10.0f;
    public float stoppingDistance = 0.5f;
    public float timeAsleep = 1.0f;
    public float timeAwake = 1.5f;
    
    // for bounce pattern
    public float bounceRadius;
    public int numberOfBounces = 2;

    List<FaceEnemy> spawnedEnemies; //when enemies are spawned, all are added to this list. if destroyed, the entry will be null.
    List<Vector3> spawnPositions;

	// Use this for initialization
	void Start () {
        spawnedEnemies = new List<FaceEnemy>();
	}

    public void SpawnFaces()
    {
        spawnPositions = GetPositions(SpawnPattern);
        foreach (Vector3 position in spawnPositions)
        {
            FaceEnemy enemy = CreateEnemy(position);
            InitializeEnemy(enemy);
        }
        StartCoroutine(ActivateEnemies());
    }

    public List<FaceEnemy> getSpawnedEnemies()
    {
        return spawnedEnemies;
    }

    public void SetAttributes(Wave wave)
    {
        this.FaceEnemyPrefab = wave.FaceEnemyPrefab;
        this.target = wave.target;

        this.SpawnPattern = wave.SpawnPattern;
        this.numberOfEnemies = wave.numberOfEnemies;
        this.activeDelayTime = wave.activeDelayTime;

        // Sphere spawning stuff
        this.minSpawnDistanceFromCenter = wave.minSpawnDistanceFromCenter;
        this.maxSpawnDistanceFromCenter = wave.maxSpawnDistanceFromCenter;
        // Line spawning stuff
        this.maxLineLength = wave.maxLineLength;

        this.SpawnBehaviour = wave.SpawnBehaviour;
        this.attackSpeed = wave.attackSpeed;
        this.stoppingDistance = wave.stoppingDistance;
        this.timeAsleep = wave.timeAsleep;
        this.timeAwake = wave.timeAwake;

        // for bounce pattern
        this.bounceRadius = wave.bounceRadius;
        this.numberOfBounces = wave.numberOfBounces;
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

    void InitializeEnemy(FaceEnemy enemy)
    {
        enemy.target = this.target;
        enemy.timeAsleep = this.timeAsleep;
        enemy.timeAwake = this.timeAwake;
        enemy.attackSpeed = this.attackSpeed;
        enemy.stoppingDistance = this.stoppingDistance;
        enemy.SetBehavior(GetEnemyBehaviour(enemy, SpawnBehaviour));
    }

    IEnumerator ActivateEnemies()
    {
        yield return new WaitForSeconds(activeDelayTime);
        foreach(FaceEnemy enemy in spawnedEnemies)
        {
            enemy.gameObject.SetActive(true);
            enemy.Activate();
        }
    }
    
    List<Vector3> GetPositions(SPAWN_PATTERNS pattern)
    {
        switch (pattern)
        {
            case SPAWN_PATTERNS.SPHERE:
                return VectorPositions.GetPositionsInASphere(transform.position, numberOfEnemies, minSpawnDistanceFromCenter, maxSpawnDistanceFromCenter);
            case SPAWN_PATTERNS.VERTICAL_LINE:
                return VectorPositions.GetPositionsInAVerticalLine(transform.position, numberOfEnemies, maxLineLength);
            default:
                return null;
        }
    }

    IEnumerator GetEnemyBehaviour(FaceEnemy enemy, SPAWN_BEHAVIOURS behaviour)
    {
        switch (behaviour)
        {
            case SPAWN_BEHAVIOURS.WAKE_THEN_ATTACK:
                return FaceEnemyBehaviours.WakeThenAttack(enemy, target.transform);
            case SPAWN_BEHAVIOURS.TRAVEL_BACK_AND_FORTH_THEN_ATTACK:
                return FaceEnemyBehaviours.TravelBackAndForthThenAttack(enemy, target.transform, bounceRadius, numberOfBounces);
            default:
                return null;
        }
    }
}
