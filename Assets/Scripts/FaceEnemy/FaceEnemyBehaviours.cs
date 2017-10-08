using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FaceEnemyBehaviours  {


    static void SwitchToRandomFace(FaceEnemy face, Material[] mats)
    {
        int index = UnityEngine.Random.Range(0, mats.Length);
        face.SwitchMaterial(mats[index]);
    }

    static void Despawn(FaceEnemy face)
    {
        face.keepFacingPlayer = false;
        face.gameObject.SetActive(false);
    }

    public static IEnumerator DirectMoveTo(FaceEnemy face, Vector3 target)
    {
        // move FaceEnemy towards target
        while (Vector3.Distance(face.transform.position, target) > face.stoppingDistance)
        {
            face.transform.position = Vector3.MoveTowards(face.transform.position, target, face.attackSpeed * Time.deltaTime);
            yield return null;
        }
    }


    public static IEnumerator WakeThenAttack(FaceEnemy face, Transform target)
    {
        yield return new WaitForSeconds(face.timeAsleep);
        SwitchToRandomFace(face, face.awakeMats);
        
        yield return new WaitForSeconds(face.timeAwake);
        SwitchToRandomFace(face, face.chargingMats);

        yield return DirectMoveTo(face, target.position);

        Despawn(face);
    }

    /// <summary>
    /// Moves the face back and forth between its position and the x and z of the specified bounce point, then makes it attack the target once the max
    /// number of bounces has been reached.
    /// 
    /// Set it to -1 so that it bounces forever!
    /// </summary>
    public static IEnumerator TravelBackAndForthThenAttack(FaceEnemy face, Transform target, Vector3 bouncePoint, int numberOfBounces = 1)
    {
        bool goingToBouncePoint = true;
        Vector3 startPoint = face.transform.position;
        Vector3 xAndZBouncePoint = new Vector3(bouncePoint.x, face.transform.position.y, bouncePoint.z);

        yield return new WaitForEndOfFrame();
        int counter = 0;
        while (counter != numberOfBounces)
        {
            SwitchToRandomFace(face, face.awakeMats);

            Vector3 destination = goingToBouncePoint ? xAndZBouncePoint : startPoint;
            yield return DirectMoveTo(face, destination);
            goingToBouncePoint = !goingToBouncePoint;

            counter++;
        }

        SwitchToRandomFace(face, face.chargingMats);
        yield return DirectMoveTo(face, target.position);
        Despawn(face);
    }

    public static IEnumerator SpiralThenAttack(FaceEnemy face, Transform target, Vector3 spiralCenter)
    {
        float radius = Vector3.Distance(face.transform.position, spiralCenter);
        float angle = radius;
        float angleSpeed = 20f;
        float radialSpeed = 5f;

        while(Vector3.Distance(face.transform.position, spiralCenter) > 0.5f)
        {
            angle += Time.deltaTime * angleSpeed;
            radius -= Time.deltaTime * radialSpeed;

            float x = radius * Mathf.Cos(Mathf.Deg2Rad * angle);
            float y = radius * Mathf.Sin(Mathf.Deg2Rad * angle);
            float z = face.transform.position.z;

            face.transform.position = new Vector3(x, y, z);
            yield return null;
        }
    }
}
