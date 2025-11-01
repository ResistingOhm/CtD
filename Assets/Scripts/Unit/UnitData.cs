using UnityEngine;
[System.Serializable]

public class UnitData
{
    [Header("-Basic")]
    public string unitName;
    public int unitID;
    public int[] unitSynergy;
    public int unitCost;

    [Header("-Main Status")] //Increase when Level Up
    public int baseHealth;
    public int baseAttack;
    public int baseDefense;
    public float baseAttackSpeed;

    [Header("-Minor Status")] //Don't increase when Level Up
    public int baseHealthRegen;
    public int baseLifeSteal;
    public int baseEvade;
    public int baseRange;

    [Header("-Level")]
    public float[] healthRate = new float[3];
    public float[] attackRate = new float[3];
    public float[] defenseRate = new float[3];
    public float[] attackSpeedRate = new float[3];
}
