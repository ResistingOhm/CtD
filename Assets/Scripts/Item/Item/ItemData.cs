using UnityEngine;
using static UnitData;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("-Basic")]
    public string itemName;
    public int itemCost;
    public Sprite itemSprite;

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
