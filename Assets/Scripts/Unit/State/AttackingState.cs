using System.Collections;
using UnityEngine;

/// <summary>
/// Attacking mean unit is attacking target which is in their attack range
/// </summary>
public class AttackingState : IUnitState
{
    private Unit unit;
    private float currentTime;
    private float attackInterval;

    public AttackingState(Unit u)
    {
        this.unit = u;
    }

    public void Enter()
    {
        //Set Animation or Sprite to Attack
        currentTime = 0f;
        attackInterval = 1f / unit.GetAttackSpeed();
    }

    public void Update()
    {
        currentTime += 0.1f;

        if (unit.target == null)
        {
            unit.SetState(unit.idleState);
            return;
        }

        if (unit.target.GetCurrentHealth() <= 0)
        {
            unit.SetTarget();
            return;
        }

        if (!unit.IsTargetInRange())
        {
            unit.SetState(unit.idleState);
            return;
        }

        if (currentTime > attackInterval)
        {
            currentTime = 0f;
            var dealt = unit.target.GetDamage(unit.GetAttack());
            Debug.Log(dealt);
            unit.GainHealth(dealt * unit.GetLifeSteal() / 100);
        }
    }

    public void Exit()
    {
        //nothing happen?
    }

}
