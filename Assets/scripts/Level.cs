using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "level", menuName = "levels/Create Level", order = 1)]
public class Level:ScriptableObject
{
    [Serializable]
    public class Round
    {
        [SerializeField] public int[] enemyType;
        [SerializeField] public int[] enemyCount;
        [SerializeField] public float enemyTypeSpawnGap;
        [SerializeField] public float spawnGap;

    }
    [SerializeField] public Round[] rounds;
    [SerializeField] public int startingCoins;
    [SerializeField] public int startingHealth;
    [SerializeField] public Vector3[] path;
    [SerializeField] public float cameraSize;
}
