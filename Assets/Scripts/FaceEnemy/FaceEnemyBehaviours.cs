using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FaceEnemyBehaviours  {
    public enum SPAWN_BEHAVIOURS
    {
        WAKE_THEN_ATTACK,
        TRAVEL_BACK_AND_FORTH_THEN_ATTACK
    }

    static void SwitchToRandomFace(FaceEnemy face, Material[] mats)
    {
        int index = UnityEngine.Random.Range(0, mats.Length);
        face.SwitchMaterial(mats[index]);
    }

    static void Despawn(FaceEnemy face)
    {
        face.keepFacingPlayer = false;
        face.gameObject.SetActive(false);
        GameObject.Destroy(face.gameObject);
    }

    public static IEnumerator DirectMoveTo(FaceEnemy face, Vector3 target)
    {
        // move FaceEnemy towards target
        while (Vector3.Distance(face.transform.position, target) > face.stoppingDistance)
        {
            face.transform.position = Vector3.MoveTowards(face.transform.position, target, face.chargeSpeed * Time.deltaTime);
            yield return null;
        }
    }

    
    public static IEnumerator StartDamageAnimation(FaceEnemy face, float duration)
    {
        Material originalFace = face.GetCurrentFace();
        SwitchToRandomFace(face, face.spawningMats);
        yield return new WaitForSeconds(duration);
        face.SwitchMaterial(originalFace);
        yield return null;
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
    /// Moves the face back and forth around its bounce point, then makes it attack the target once the max
    /// number of bounces has been reached.
    /// 
    /// Set it to -1 so that it bounces forever!
    /// </summary>
    public static IEnumerator TravelBackAndForthThenAttack(FaceEnemy face, Transform target, float radius, int numberOfBounces = 1)
    {
        bool goingToBouncePoint = true;
        Vector3 point1 = new Vector3(face.transform.position.x + radius, face.transform.position.y, face.transform.position.z);
        Vector3 point2 = new Vector3(face.transform.position.x - radius, face.transform.position.y, face.transform.position.z);

        yield return new WaitForEndOfFrame();
        int counter = 0;
        while (counter != numberOfBounces)
        {
            SwitchToRandomFace(face, face.awakeMats);

            Vector3 destination = goingToBouncePoint ? point2 : point1;
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
