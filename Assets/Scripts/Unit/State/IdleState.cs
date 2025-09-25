using UnityEngine;


/// <summary>
/// Idle mean unit has target, but it can't move(no path) and can't attack(out of range)
/// </summary>
public class IdleState : IUnitState
{
    private Unit unit;

    public IdleState(Unit u)
    {
        this.unit = u;
    }


    public void Enter()
    {
        //Set Animation or Sprite to Idle
    }
    public void FixedUpdate()
    {
        //Set Target Phase ---------------------
        //if unit's target is null?
            //if false(unit already have target), if target is in attack range?
                //if true, attack.
                //if false, now unit lost target. set target null.
            //if true(unit didn't have target yet), find closest enemy unit and save it as target
                //if target is in attack range, attack.
                //if taget is out of attack range, move.(PathFinding)
    }

    public void Update()
    {
        /*
        if () //check unit hp is lower than 0
        {
            //unit.SetState(unit.deadState);
            return;
        }

        if () //check unit has target and there is path, but out of attack range
        {
            //unit.SetState(unit.movingState);
            return;
        }

        if () //there is target in attack range
        {
            //unit.SetState(unit.attackingState);
        }
        */
    }

    public void Exit()
    {
        //Nothing happen?
    }
}
