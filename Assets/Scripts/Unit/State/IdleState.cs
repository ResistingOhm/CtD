using UnityEngine;
using static UnityEngine.InputManagerEntry;


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

    public void Update()
    {
        if (unit.target == null)
        {
            unit.SetTarget();
            return;
        }

        if (unit.IsTargetInRange())
        {
            unit.SetState(unit.attackingState);
            return;
        }

        ChessGrid grid = unit.FindNextGrid();

        if (grid != null)
        {
            grid.cost = byte.MaxValue;
            unit.currentPos.cost = 1;
            unit.currentPos = grid;
            unit.SetState(unit.movingState);
            return;
        }

    }

    public void Exit()
    {
        //Nothing happen?
    }
}
