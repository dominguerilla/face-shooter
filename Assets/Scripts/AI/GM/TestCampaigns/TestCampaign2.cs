using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using System;

/// <summary>
/// Each wave should only appear after all of the monsters in the previous wave are killed.
/// </summary>
public class TestCampaign2 : Campaign
{

    public int maxExtraBounces = 3;

    public override void InitializeEnemy(Wave currentWave, FaceEnemy enemy)
    {
        enemy.target = currentWave.target;
        enemy.health = currentWave.health;
        enemy.timeAsleep = currentWave.timeAsleep;
        enemy.timeAwake = currentWave.timeAwake;
        enemy.chargeSpeed = currentWave.attackSpeed;
        enemy.stoppingDistance = currentWave.stoppingDistance;

        // random affinity for enemies
        FaceEnemy.COLOR randAffinity = (FaceEnemy.COLOR)UnityEngine.Random.Range(1.0f, Enum.GetValues(typeof(FaceEnemy.COLOR)).Length);
        enemy.affinity = randAffinity;
        
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
