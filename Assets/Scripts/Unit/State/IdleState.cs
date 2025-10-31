using UnityEngine;


/// <summary>
/// Idle mean unit don't have target, or it can't move(no path) and can't attack(out of range)
/// This unit should find target(closest enemy) and attack or find path to it
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
        //Nothing happen?
    }

    public void Update()
    {
        /*
         
        if () //if target is null
        {
            find target
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
            //unit.SetState(unit.movingState);
            return;
        }

        */
    }

    public void Exit()
    {
        //Nothing happen?
    }
}
