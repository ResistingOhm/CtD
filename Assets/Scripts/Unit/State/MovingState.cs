using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
        unit.SetVelocity(2f);
    }

    public void Update()
    {
        //If unit arrive at way point
        unit.SetState(unit.idleState);
    }

    public void Exit()
    {
        unit.SetVelocity(0f);
        unit.transform.position = unit.currentPos.transform.position;
    }
}
