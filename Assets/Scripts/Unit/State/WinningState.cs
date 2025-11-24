using UnityEngine;

public class WinningState : IUnitState
{
    private Unit unit;

    public WinningState(Unit u)
    {
        this.unit = u;
    }

    public void Enter()
    {
        //Set Animation or Sprite to Idle
    }

    public void Exit()
    {
        //Nothing happen?
    }

    public void Update()
    {
        //Nothing happen?
    }
}
