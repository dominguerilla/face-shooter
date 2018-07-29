using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using System;

/// <summary>
/// This campaign assigns random affinities to spawned enemies, as well as adding a random number of bounces to enemies that have the TRAVEL_BACK_AND_FORTH behavior.
/// This campaign also assigns different materials to enemies with different affinities.
/// 
/// </summary>
public class TestCampaign2 : Campaign
{

    public int maxExtraBounces = 3;

    /// <summary>
    /// 0 - 2 should be the sleep faces; 3 - 5 should be the awake faces; 6 - 8 should be the charging faces
    /// </summary>
    public Material[] redAffinityMats;
    
    /// <summary>
    /// 0 - 2 should be the sleep faces; 3 - 5 should be the awake faces; 6 - 8 should be the charging faces
    /// </summary>
    public Material[] blueAffinityMats;

    public override void InitializeEnemy(Wave currentWave, FaceEnemy enemy)
    {
        base.InitializeEnemy(currentWave, enemy);

        // random affinity for enemies
        FaceEnemy.COLOR randAffinity = (FaceEnemy.COLOR)UnityEngine.Random.Range(1.0f, Enum.GetValues(typeof(FaceEnemy.COLOR)).Length);
        enemy.affinity = randAffinity;
        
        // materials based on affinity assigned
        // defaults to red materials
        if(randAffinity == FaceEnemy.COLOR.BLUE) {
            Material[] sleepMats = new Material[3] { blueAffinityMats[0], blueAffinityMats[1], blueAffinityMats[2] };
            Material[] awakeMats = new Material[3] { blueAffinityMats[3], blueAffinityMats[4], blueAffinityMats[5] };
            Material[] chargeMats = new Material[3] { blueAffinityMats[6], blueAffinityMats[7], blueAffinityMats[8] };
            enemy.spawningMats = sleepMats;
            enemy.awakeMats = awakeMats;
            enemy.chargingMats = chargeMats;
        }else {
            Material[] sleepMats = new Material[3] { redAffinityMats[0], redAffinityMats[1], redAffinityMats[2] };
            Material[] awakeMats = new Material[3] { redAffinityMats[3], redAffinityMats[4], redAffinityMats[5] };
            Material[] chargeMats = new Material[3] { redAffinityMats[6], redAffinityMats[7], redAffinityMats[8] };
            enemy.spawningMats = sleepMats;
            enemy.awakeMats = awakeMats;
            enemy.chargingMats = chargeMats;
        }
             
        // random number of bounces for enemy
        if (currentWave.SpawnBehaviour == FaceEnemyBehaviours.SPAWN_BEHAVIOURS.TRAVEL_BACK_AND_FORTH_THEN_ATTACK)
        {
            int originalNumBounces = currentWave.numberOfBounces;
            int numberOfBounces = UnityEngine.Random.Range(currentWave.numberOfBounces, currentWave.numberOfBounces + maxExtraBounces);
            currentWave.numberOfBounces = numberOfBounces;
            enemy.SetBehavior(GetEnemyBehaviour(enemy, currentWave.SpawnBehaviour, currentWave));
            currentWave.numberOfBounces = originalNumBounces;
        }else
        {
            enemy.SetBehavior(GetEnemyBehaviour(enemy, currentWave.SpawnBehaviour, currentWave));
        }
    }
}
