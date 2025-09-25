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
        //if unit's target is null?
            //if false(unit already have target), if target is in attack range?
                //if true, keep attack.
                //if false, now unit lost target. set target null.
            //if true(unit didn't have target yet), find closest enemy unit and save it as target
                //if target is in attack range, start attack.
                //if taget is out of attack range, move.
    }

    public void Update()
    {
        /*
        if () //check unit hp is lower than 0
        {
            //unit.SetState(unit.deadState);
            return;
        }

        //give target damage per attack speed

        if () //check unit is moving now
        {
            //unit.SetState(unit.movingState);
            return;
        }

        if () //there is no target in attack range
        {
            //unit.SetState(unit.idleState);
        }
        */
    }

    public void Exit()
    {
        //nothing happen?
    }
}
