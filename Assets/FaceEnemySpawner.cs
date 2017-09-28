using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FaceEnemySpawner : MonoBehaviour {

    public GameObject FaceEnemyPrefab;
    public GameObject target;
    public int numberOfEnemies = 10;
    public float spawnDelayTime = 12.0f;
    public float minSpawnDistanceFromCenter = 1.0f;
    public float maxSpawnDistanceFromCenter = 10.0f;

    // these following values are a rough attempt to synchronize with Bayonetta's 'Fly Me to the Moon'
    public float timeAsleep = 1.0f;
    public float timeAwake = 1.5f;

    public float attackSpeed = 10.0f;
    public float stoppingDistance = 0.5f;


    List<FaceEnemy> spawnedEnemies;

	// Use this for initialization
	void Start () {
        spawnedEnemies = new List<FaceEnemy>();
        Boombox boombox = GameObject.FindObjectOfType<Boombox>();
        //boombox.onPlay.AddListener(SpawnFacesInASphere);
        boombox.onPlay.AddListener(SpawnFacesInAVerticalLine);
	}

    public void SpawnFacesInASphere()
    {
        for(int i = 0; i < numberOfEnemies; i++)
        {
            Vector3 position = getRandomPositionInASphere();
            FaceEnemy enemy = CreateEnemy(position);
            InitializeEnemy(enemy);
        }
        StartCoroutine(SpawnEnemies());
    }

    public void SpawnFacesInAVerticalLine()
    {
        List<Vector3> positions = getPositionsInAVerticalLine(numberOfEnemies, 50.0f);
        foreach(Vector3 position in positions)
        {
            FaceEnemy enemy = CreateEnemy(position);
            InitializeEnemy(enemy);
        }
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(spawnDelayTime);
        foreach(FaceEnemy enemy in spawnedEnemies)
        {
            enemy.gameObject.SetActive(true);
            enemy.Activate();
        }
    }

    Vector3 getRandomPositionInASphere()
    {
        // get a random position away from center, randomly inverting it
        float x = Random.Range(minSpawnDistanceFromCenter, maxSpawnDistanceFromCenter);
        float y = Random.Range(minSpawnDistanceFromCenter, maxSpawnDistanceFromCenter);
        float z = Random.Range(minSpawnDistanceFromCenter, maxSpawnDistanceFromCenter);

        // the following return either 1 or -1
        int xInvert = Random.Range(0, 2) * 2 - 1;
        int yInvert = Random.Range(0, 2) * 2 - 1;
        int zInvert = Random.Range(0, 2) * 2 - 1;

        x *= xInvert;
        y *= yInvert;
        z *= zInvert;

        Vector3 pos = new Vector3(this.transform.position.x + x, this.transform.position.y + y, this.transform.position.z + z);
        return pos;
    }

    List<Vector3> getPositionsInAVerticalLine(int numberOfPoints, float maxLength = 10)
    {
        // establish the position of this spawner as the center of the line
        Vector3 topLimit = this.transform.position + new Vector3(0, maxLength / 2.0f, 0);
        Vector3 bottomLimit = this.transform.position - new Vector3(0, maxLength / 2.0f, 0);

        List<Vector3> positions = new List<Vector3>();
        float incrementSize = maxLength / numberOfPoints;
        Vector3 currentPosition = bottomLimit; // build from the ground up!
        for(int i = 0; i < numberOfPoints; i++)
        {
            positions.Add(currentPosition);
            currentPosition += new Vector3(0, incrementSize, 0);
        }

        return positions;
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
        //enemy.SetBehavior(FaceEnemyBehaviours.WakeThenAttack(enemy, target.transform));
        //enemy.SetBehavior(FaceEnemyBehaviours.TravelBackAndForthThenAttack(enemy, target.transform, enemy.transform.position + new Vector3(20, 0, 0), 3));

        GameObject center = GameObject.Find("Center");
        Vector3 centerPos = new Vector3(center.transform.position.x, center.transform.position.y, 0);
        enemy.SetBehavior(FaceEnemyBehaviours.SpiralThenAttack(enemy, target.transform, enemy.transform.position + centerPos));
    }
}
