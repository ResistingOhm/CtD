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
        //Disable All Collider in unit (Character Collider + waypointCollider)
        //Velocity to zero
        deadTime = 0f;
    }

    public void FixedUpdate()
    {
        //Nothing happen?
    }

    public void Update()
    {
        deadTime += Time.deltaTime;

        if (deadTime > 0.7f)
        {
            //unit disabled
        }
    }

    public void Exit()
    {
        //Nothing happen?
    }
}
