using UnityEngine;

/// <summary>
/// Moving mean unit has target and has path to it, but can't attack it(out of range)
/// </summary>
public class MovingState : IUnitState
{
    private Unit unit;

    public MovingState(Unit u)
    {
        this.unit = u;
    }

    public void Enter()
    {
        //Set Animation or Sprite to Move
    }

    public void FixedUpdate()
    {
        //Move to wayPoint
    }

    public void Update()
    {
        /*
        if () //check unit hp is lower than 0
        {
            //unit.SetState(unit.deadState);
            return;
        }

        //If unit arrive at way point
        //set unit position to center of grid.

        if () //there is target in attack range
        {
            //unit.SetState(unit.attackingState);
        }
        
        if ()//check unit is not moving now
        {
            //unit.SetState(unit.idleState);
            return;
        }
        */
    }

    public void Exit()
    {
        //Set Velocity.zero
    }
}
