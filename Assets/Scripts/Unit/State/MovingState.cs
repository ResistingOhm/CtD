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

        //If unit arrive at way point
        //set unit position to center of grid.

        if () //there is no target (target == null)
        {
            //unit.SetState(unit.idleState);
            return;
        }

        if () //there is target in attack range
        {
            //unit.SetState(unit.attackingState);
            return;
        }

        //find path
        if () //if there's path to target
        {
            //set Waypoint
            return;
        } else
        {
            //unit.SetState(unit.idleState);
        }


        */
    }

    public void Exit()
    {
        //Set Velocity.zero
    }
}
