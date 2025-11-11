using UnityEngine;

public class UnitTotalStatus: Status
{
    public int unitCurrentHealth;

    public Status unit;
    public Status[] synergy;
    public Status[] items = new Status[3];

    public void RefreshTotalStatus(bool isDeck)
    {
        maxHealth = unit.maxHealth;
        attack = unit.attack;
        defense = unit.defense;
        attackSpeed = unit.attackSpeed;

        healthRegen = unit.healthRegen;
        lifeSteal = unit.lifeSteal;
        evade = unit.evade;
        range = unit.range;

        //Apply Synergy
        if (isDeck)
        {
            for (int i = 0; i < synergy.Length; i++)
            {
                maxHealth += synergy[i].maxHealth;
                attack += synergy[i].attack;
                defense += synergy[i].defense;
                attackSpeed += synergy[i].attackSpeed;

                healthRegen += synergy[i].healthRegen;
                lifeSteal += synergy[i].lifeSteal;
                evade += synergy[i].evade;
                range += synergy[i].range;
            }
        }

        //Apply Item
        for (int i = 0; i < 3; i++)
        {
            if (items[i] == null) break;

            maxHealth += items[i].maxHealth;
            attack += items[i].attack;
            defense += items[i].defense;
            attackSpeed += items[i].attackSpeed;

            healthRegen += items[i].healthRegen;
            lifeSteal += items[i].lifeSteal;
            evade += items[i].evade;
            range += items[i].range;
        }

        unitCurrentHealth = maxHealth;
    }
}
