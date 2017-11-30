
using System.Collections.Generic;
using UnityEngine;

public class Positioner
{
    public enum SPAWN_PATTERNS
    {
        SPHERE,
        VERTICAL_LINE
    }

    /// <summary>
    /// Returns a queue of Vector3 positions relative to spawnerPosition, which are based on the given spawn pattern in the Wave.
    /// </summary>
    public Queue<Vector3> GetPositions(Wave wave, Vector3 spawnerPosition)
    {
        switch (wave.SpawnPattern)
        {
            case SPAWN_PATTERNS.SPHERE:
                return GetPositionsInASphere(wave, spawnerPosition);
            case SPAWN_PATTERNS.VERTICAL_LINE:
                return GetPositionsInAVerticalLine(wave, spawnerPosition);
            default:
                return null;
        }
    }

    public Vector3 GetRandomPositionInASphere(Wave wave, Vector3 center)
    {
        // get a random position away from center, randomly inverting it
        float x = Random.Range(wave.minSpawnDistanceFromCenter, wave.maxSpawnDistanceFromCenter);
        float y = Random.Range(wave.minSpawnDistanceFromCenter, wave.maxSpawnDistanceFromCenter);
        float z = Random.Range(wave.minSpawnDistanceFromCenter, wave.maxSpawnDistanceFromCenter);

        // the following return either 1 or -1
        int xInvert = Random.Range(0, 2) * 2 - 1;
        int yInvert = Random.Range(0, 2) * 2 - 1;
        int zInvert = Random.Range(0, 2) * 2 - 1;

        x *= xInvert;
        y *= yInvert;
        z *= zInvert;

        Vector3 pos = new Vector3(center.x + x, center.y + y, center.z + z);
        return pos;
    }

    public Queue<Vector3> GetPositionsInASphere(Wave wave, Vector3 center)
    {
        Queue<Vector3> positions = new Queue<Vector3>();
        for (int i = 0; i < wave.numberOfEnemies; i++)
        {
            positions.Enqueue(GetRandomPositionInASphere(wave, center));
        }
        return positions;
    }

    public Queue<Vector3> GetPositionsInAVerticalLine(Wave wave, Vector3 center)
    {
        // establish the position of this spawner as the center of the line
        //Vector3 topLimit = center + new Vector3(0, maxLength / 2.0f, 0);
        Vector3 bottomLimit = center - new Vector3(0, wave.maxLineLength / 2.0f, 0);

        Queue<Vector3> positions = new Queue<Vector3>();
        float incrementSize = wave.maxLineLength / wave.numberOfEnemies;
        Vector3 currentPosition = bottomLimit; // build from the ground up!
        for (int i = 0; i < wave.numberOfEnemies; i++)
        {
            positions.Enqueue(currentPosition);
            currentPosition += new Vector3(0, incrementSize, 0);
        }

        return positions;
    }
}