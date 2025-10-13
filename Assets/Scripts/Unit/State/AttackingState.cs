using UnityEngine;

/// <summary>
/// Attacking mean unit is attacking target which is in their attack range
/// </summary>
public class AttackingState : IUnitState
{
    private Unit unit;

    public AttackingState(Unit u)
    {
        this.unit = u;
    }

    public void Enter()
    {
        //Set Animation or Sprite to Attack
    }

    public void FixedUpdate()
    {
        //nothing happen?
    }

    public void Update()
    {
        /*
        if () //check unit hp is lower than 0
        {
            //unit.SetState(unit.deadState);
            return;
        }

        if () //there is no target in attack range (target == null || target is out of range)
        {
            //unit.SetState(unit.idleState);
            return;
        }

        //give target damage per attack speed

        */
    }

    public void Exit()
    {
        //nothing happen?
    }
}
