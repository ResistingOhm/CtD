using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// Moving mean unit has target and has path to it, but can't attack it(out of range)
/// </summary>
public class MovingState : IUnitState
{
    private Unit unit;
    private Vector2 dir;

    public MovingState(Unit u)
    {
        this.unit = u;
    }

    public void Enter()
    {
        //Set Animation or Sprite to Move
        dir = unit.SetVelocity(2.5f);

        if (dir.y > 0) unit.ChangeSprite(MovDir.UPM);
        if (dir.y < 0) unit.ChangeSprite(MovDir.DOWNM);
        if (dir.x > 0) unit.ChangeSprite(MovDir.RIGHTM);
        if (dir.x < 0) unit.ChangeSprite(MovDir.LEFTM);
    }

    public void Update()
    {
        if (Vector2.Distance(unit.transform.position, unit.currentPos.transform.position) < 0.1f) unit.SetState(unit.idleState);
        if (Vector2.Distance(unit.transform.position, unit.currentPos.transform.position) > 1.1f) unit.SetState(unit.idleState);
    }

    public void Exit()
    {
        unit.SetVelocity(0f);
        unit.transform.position = unit.currentPos.transform.position;

        if (dir.y > 0) unit.ChangeSprite(MovDir.UPI);
        if (dir.y < 0) unit.ChangeSprite(MovDir.DOWNI);
        if (dir.x > 0) unit.ChangeSprite(MovDir.RIGHTI);
        if (dir.x < 0) unit.ChangeSprite(MovDir.LEFTI);
    }
}
