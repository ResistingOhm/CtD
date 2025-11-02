using System.Collections;
using UnityEngine;

/// <summary>
/// Attacking mean unit is attacking target which is in their attack range
/// </summary>
public class AttackingState : IUnitState
{
    private Unit unit;
    private float currentTime;

    public AttackingState(Unit u)
    {
        this.unit = u;
    }

    public void Enter()
    {
        //Set Animation or Sprite to Attack
        currentTime = 0f;
    }

    public void Update()
    {
        currentTime += 0.1f;

        if (unit.target == null || !unit.IsTargetInRange())
        {
            unit.SetState(unit.idleState);
            return;
        }

        if (unit.target.unitCurrentHealth <= 0)
        {
            unit.SetTarget();
            return;
        }

        if (currentTime > 1 / unit.GetAttackSpeed())
        {
            currentTime = 0f;
            var dealt = unit.target.GetDamage(unit.GetAttack());
            unit.GainHealth(dealt * unit.GetLifeSteal() / 100);
        }
    }

    public void Exit()
    {
        //nothing happen?
    }

}
