using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "level", menuName = "towers/Create Details", order = 1)]
public class TowerDetails : ScriptableObject
{
    [Serializable]
    public class Upgrades
    {
        [SerializeField] public float damageMultiplier;
        [SerializeField] public float speedMultiplier;
        [SerializeField] public float areaMultiplier;
        [SerializeField] public int cost;
    }

    [Serializable]
    public class Tower
    {
        [SerializeField] public Upgrades[] levels;
        [SerializeField] public float cost;
        [SerializeField] public float sellPercent;
    }
    [SerializeField] public Tower[] towers;

}
