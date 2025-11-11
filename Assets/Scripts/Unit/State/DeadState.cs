using UnityEngine;

/// <summary>
/// Dead mean unit's hp gone
/// </summary>
public class DeadState: IUnitState
{
    private Unit unit;
    private float deadTime = 0f;

    public DeadState(Unit u)
    {
        this.unit = u;
    }

    public void Enter()
    {
        //Set Animation or Sprite to Dead
        unit.SetVelocity(0);
        unit.currentPos.cost = 1;
        unit.currentPos = null;
        deadTime = 0f;
    }

    public void Update()
    {
        deadTime += 0.1f;

        if (deadTime > 0.2f)
        {
            unit.gameObject.SetActive(false);
        }
    }

    public void Exit()
    {
        //Nothing happen?
    }
}
