
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
        float distance = Random.Range(wave.minSpawnDistanceFromCenter, wave.maxSpawnDistanceFromCenter);
        Vector3 pos = center + Random.insideUnitSphere * distance;
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