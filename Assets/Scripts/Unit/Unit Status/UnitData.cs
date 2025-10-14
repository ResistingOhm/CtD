using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class UnitData : ScriptableObject
{
    public enum UnitGroup
    {
        None,
        Royal,
        Forest,
        Undead,
        Hero,
    }
    public enum UnitType
    {
        Knight,
        Warrior,
        Archer,
        Magician,
    }

    [Header("-Basic")]
    public UnitGroup unitGroup;
    public UnitType unitType;
    public string unitName;
    public int unitCost;
    public Sprite unitSprite;

    [Header("-Main Status")] //Increase when Level Up
    public int baseHealth;
    public int baseAttack;
    public int baseDefense;
    public int baseAttackSpeed;

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
