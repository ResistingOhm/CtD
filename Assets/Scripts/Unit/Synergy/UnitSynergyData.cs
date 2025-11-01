using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitSynergyData
{
    [Header("-Basic")]
    public string synergyName;
    public int synergyID;
    public int[] requiredCounts;

    [Header("-Status")]
    public int[] health;
    public int[] attack;
    public int[] defense;
    public float[] attackSpeed;

    [Header("-Minor Status")]
    public int[] healthRegen;
    public int[] lifeSteal;
    public int[] evade;
    public int[] range;
}


