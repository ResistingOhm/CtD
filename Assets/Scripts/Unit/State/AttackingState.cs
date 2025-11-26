using System.Collections;
using UnityEngine;

/// <summary>
/// Attacking mean unit is attacking target which is in their attack range
/// </summary>
public class AttackingState : IUnitState
{
    private Unit unit;
    private float currentTime;
    private float attackInterval;

    private Vector2 dir;

    public AttackingState(Unit u)
    {
        this.unit = u;
    }

    public void Enter()
    {
        //Set Animation or Sprite to Attack
        currentTime = 0f;
        attackInterval = 1f / unit.GetAttackSpeed();

        dir = unit.target.currentPos.gridIndex - unit.currentPos.gridIndex;

        if (dir.y > 0) unit.ChangeSprite(MovDir.UPM);
        if (dir.y < 0) unit.ChangeSprite(MovDir.DOWNM);
        if (dir.x > 0) unit.ChangeSprite(MovDir.RIGHTM);
        if (dir.x < 0) unit.ChangeSprite(MovDir.LEFTM);
    }

    public void Update()
    {
        currentTime += 0.1f;

        if (unit.target == null)
        {
            unit.SetState(unit.idleState);
            return;
        }

        if (unit.target.GetCurrentHealth() <= 0)
        {
            unit.SetTarget();
            return;
        }

        if (!unit.IsTargetInRange())
        {
            unit.SetState(unit.idleState);
            return;
        }

        if (currentTime > attackInterval)
        {
            currentTime = 0f;
            var dealt = unit.target.GetDamage(unit.GetAttack());
            unit.GainHealth(dealt * unit.GetLifeSteal() / 100);
        }
    }

    public void Exit()
    {
        if (dir.y > 0) unit.ChangeSprite(MovDir.UPI);
        if (dir.y < 0) unit.ChangeSprite(MovDir.DOWNI);
        if (dir.x > 0) unit.ChangeSprite(MovDir.RIGHTI);
        if (dir.x < 0) unit.ChangeSprite(MovDir.LEFTI);
    }

}
