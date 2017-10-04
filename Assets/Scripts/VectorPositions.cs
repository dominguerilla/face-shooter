using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorPositions {

    public static Vector3 GetRandomPositionInASphere(Vector3 center, float minSpawnDistanceFromCenter, float maxSpawnDistanceFromCenter)
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

        Vector3 pos = new Vector3(center.x + x, center.y + y, center.z + z);
        return pos;
    }

    public static List<Vector3> GetPositionsInASphere(Vector3 center, int numberOfPoints, float minSpawnDistanceFromCenter, float maxSpawnDistanceFromCenter)
    {
        List<Vector3> positions = new List<Vector3>();
        for(int i = 0; i < numberOfPoints; i++)
        {
            positions.Add(GetRandomPositionInASphere(center, minSpawnDistanceFromCenter, maxSpawnDistanceFromCenter));
        }
        return positions;
    }

    public static List<Vector3> GetPositionsInAVerticalLine(Vector3 center, int numberOfPoints, float maxLength)
    {
        // establish the position of this spawner as the center of the line
        Vector3 topLimit = center + new Vector3(0, maxLength / 2.0f, 0);
        Vector3 bottomLimit = center - new Vector3(0, maxLength / 2.0f, 0);

        List<Vector3> positions = new List<Vector3>();
        float incrementSize = maxLength / numberOfPoints;
        Vector3 currentPosition = bottomLimit; // build from the ground up!
        for (int i = 0; i < numberOfPoints; i++)
        {
            positions.Add(currentPosition);
            currentPosition += new Vector3(0, incrementSize, 0);
        }

        return positions;
    }
}
