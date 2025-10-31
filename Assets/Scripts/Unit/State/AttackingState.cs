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
        //give target damage per attack speed

        if (unit.target == null || !unit.IsTargetInRange())
        {
            unit.SetState(unit.idleState);
            return;
        }

        if (currentTime > unit.GetAttackSpeed())
        {
            currentTime = 0f;
            //Attack
        }

        currentTime += Time.deltaTime;
    }

    public void Exit()
    {
        //nothing happen?
    }

}
