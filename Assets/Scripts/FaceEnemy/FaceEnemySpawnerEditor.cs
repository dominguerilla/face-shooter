using System.Collections;
using System.Collections.Generic;
using UnityEditor;

//[CustomEditor(typeof(FaceEnemySpawner))]
[CanEditMultipleObjects]
public class FaceEnemySpawnerEditor : Editor {

    SerializedProperty p_FaceEnemyPrefab;
    SerializedProperty p_target;
    SerializedProperty p_SpawnPattern;
    SerializedProperty p_numberOfEnemies;
    SerializedProperty p_activeDelayTime;
    SerializedProperty p_SpawnBehaviour;
    SerializedProperty p_bouncePoint;
    SerializedProperty p_numberOfBounces;
    SerializedProperty p_minSpawnDistanceFromCenter;
    SerializedProperty p_maxSpawnDistanceFromCenter;
    SerializedProperty p_maxLineLength;
    SerializedProperty p_attackSpeed;
    SerializedProperty p_stoppingDistance;
    SerializedProperty p_timeAsleep;
    SerializedProperty p_timeAwake;

    private void OnEnable()
    {
        p_FaceEnemyPrefab = this.serializedObject.FindProperty("FaceEnemyPrefab");
        p_target = this.serializedObject.FindProperty("target");
        p_SpawnPattern = this.serializedObject.FindProperty("SpawnPattern");
        p_numberOfEnemies = this.serializedObject.FindProperty("numberOfEnemies");
        p_activeDelayTime = this.serializedObject.FindProperty("activeDelayTime");
        p_SpawnBehaviour = this.serializedObject.FindProperty("SpawnBehaviour");
        p_bouncePoint = this.serializedObject.FindProperty("bouncePoint");
        p_numberOfBounces = this.serializedObject.FindProperty("numberOfBounces");
        p_minSpawnDistanceFromCenter = this.serializedObject.FindProperty("minSpawnDistanceFromCenter");
        p_maxSpawnDistanceFromCenter = this.serializedObject.FindProperty("maxSpawnDistanceFromCenter");
        p_maxLineLength = this.serializedObject.FindProperty("maxLineLength");
        p_attackSpeed = this.serializedObject.FindProperty("attackSpeed");
        p_stoppingDistance = this.serializedObject.FindProperty("stoppingDistance");
        p_timeAsleep = this.serializedObject.FindProperty("timeAsleep");
        p_timeAwake = this.serializedObject.FindProperty("timeAwake");
    }
    /*
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var spawner = target as FaceEnemySpawner;

        EditorGUILayout.PropertyField(p_FaceEnemyPrefab);
        EditorGUILayout.PropertyField(p_target);

        // Spawn Patterns
        EditorGUILayout.LabelField("Spawn Pattern", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(p_numberOfEnemies);
        EditorGUILayout.PropertyField(p_activeDelayTime);

        //spawner.SpawnPattern = (FaceEnemySpawner.SPAWN_PATTERNS) EditorGUILayout.EnumPopup("Spawn Pattern", spawner.SpawnPattern);
        EditorGUILayout.PropertyField(p_SpawnPattern);
        switch (p_SpawnPattern.enumValueIndex)
        {
            case (int)FaceEnemySpawner.SPAWN_PATTERNS.SPHERE:
                EditorGUILayout.PropertyField(p_minSpawnDistanceFromCenter);
                EditorGUILayout.PropertyField(p_maxSpawnDistanceFromCenter);
                break;
            case (int)FaceEnemySpawner.SPAWN_PATTERNS.VERTICAL_LINE:
                EditorGUILayout.PropertyField(p_maxLineLength);
                break;
            default:
                break;
        }

        EditorGUILayout.LabelField("Enemy Behaviour", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(p_SpawnBehaviour);
        switch (p_SpawnBehaviour.enumValueIndex)
        {
            case (int)FaceEnemySpawner.SPAWN_BEHAVIOURS.TRAVEL_BACK_AND_FORTH_THEN_ATTACK:
                EditorGUILayout.PropertyField(p_bouncePoint);
                EditorGUILayout.PropertyField(p_numberOfBounces);
                break;
            default:
                break;
        }

        EditorGUILayout.PropertyField(p_attackSpeed);
        EditorGUILayout.PropertyField(p_stoppingDistance);
        EditorGUILayout.PropertyField(p_timeAsleep);
        EditorGUILayout.PropertyField(p_timeAwake);

        serializedObject.ApplyModifiedProperties();
    }
    */
}
