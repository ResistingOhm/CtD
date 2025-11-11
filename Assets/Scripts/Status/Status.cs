using Unity.VisualScripting;
using UnityEngine;

public class Status
{
    [Header("-Main Status")] //Increase when Level Up
    public int maxHealth = 0;
    public int attack = 0;
    public int defense = 0;
    public float attackSpeed = 0;

    [Header("-Minor Status")] //Don't increase when Level Up
    public int healthRegen = 0;
    public int lifeSteal = 0;
    public int evade = 0;
    public int range = 0;
    

    public static Status Converter(UnitData u, int level)
    {
        return new Status
        {
            maxHealth = (int) (u.baseHealth * u.healthRate[level]),
            attack = (int) (u.baseAttack * u.attackRate[level]),
            defense = (int)(u.baseDefense * u.defenseRate[level]),
            attackSpeed = (int) (u.baseAttackSpeed * u.attackSpeedRate[level]),
            healthRegen = u.baseHealthRegen,
            lifeSteal = u.baseLifeSteal,
            evade = u.baseEvade,
            range = u.baseRange,
        };
    }

    public static Status Converter(UnitSynergyData u, int level)
    {
        return new Status
        {
            maxHealth = u.health[level],
            attack = u.attack[level],
            defense = u.defense[level],
            attackSpeed = u.attackSpeed[level],
            healthRegen = u.healthRegen[level],
            lifeSteal = u.lifeSteal[level],
            evade = u.evade[level],
            range = u.range[level],
        };
    }

    public static Status Converter(ItemData u)
    {
        return new Status
        {
            maxHealth = u.health,
            attack = u.attack,
            defense = u.defense,
            attackSpeed = u.attackSpeed,
            healthRegen = u.healthRegen,
            lifeSteal = u.lifeSteal,
            evade = u.evade,
            range = u.range,
        };
    }
}
