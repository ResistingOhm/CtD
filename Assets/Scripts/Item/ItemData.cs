using UnityEngine;
using static UnitData;

[System.Serializable]
public class ItemData
{
    [Header("-Basic")]
    public string itemName;
    public int itemID;
    public int itemCost;

    [Header("-Status")]
    public int health;
    public int attack;
    public int defense;
    public int attackSpeed;

    [Header("-Minor Status")]
    public int healthRegen;
    public int lifeSteal;
    public int evade;
    public int range;
}
