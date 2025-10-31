using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]
    private UnitData unitData;
    [Range(0, 2)]
    private int level;
    private ItemData[] itemData = new ItemData[3];

    [HideInInspector]
    public ChessGrid currentPos;
    [HideInInspector]
    public Unit target;

    private Rigidbody2D rb;

    [Header("-Main Status")] //Increase when Level Up
    private int unitMaxHealth;
    private int unitCurrentHealth;
    private int unitAttack;
    private int unitDefense;
    private int unitAttackSpeed;

    [Header("-Minor Status")] //Don't increase when Level Up
    private int unitHealthRegen;
    private int unitLifeSteal;
    private int unitEvade;
    private int unitRange;

    [Header("-State")]
    private IUnitState currentState;
    public IdleState idleState;
    public AttackingState attackingState;
    public MovingState movingState;
    public DeadState deadState;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        currentState = idleState;
        RefreshStatus();
    }

    void Update()
    {
        currentState.Update();
    }

    public int GetAttackSpeed()
    {
        return unitAttackSpeed;
    }

    public UnitGroup GetUnitGroup()
    {
        return unitData.unitGroup;
    }
    public UnitType GetUnitType()
    {
        return unitData.unitType;
    }

    private void RefreshStatus()
    {
        unitMaxHealth = (int) (unitData.baseHealth * unitData.healthRate[level]);
        unitAttack = (int) (unitData.baseAttack * unitData.attackRate[level]);
        unitDefense = (int) (unitData.baseDefense * unitData.defenseRate[level]);
        unitAttackSpeed = (int) (unitData.baseAttackSpeed * unitData.attackSpeedRate[level]);

        unitHealthRegen = unitData.baseHealthRegen;
        unitLifeSteal = unitData.baseLifeSteal;
        unitEvade = unitData.baseEvade;
        unitRange = unitData.baseRange;

        //Apply Group
        //Apply Type

        foreach(ItemData item in itemData)
        {
            unitMaxHealth += item.health;
            unitAttack += item.attack;
            unitDefense += item.defense;
            unitAttackSpeed += item.attackSpeed;

            unitHealthRegen += item.healthRegen;
            unitLifeSteal += item.lifeSteal;
            unitEvade += item.evade;
            unitRange += item.range;
        }
    }

    public void SetVelocity(float p)
    {
        Vector2 dir = currentPos.transform.position - transform.position;
        rb.linearVelocity = dir.normalized * p;
    }

    public void SetTarget()
    {


    }
    public void GetDamage(int damage)
    {
        int i = damage - damage * unitDefense / 100;
        if (i > 0) unitCurrentHealth -= i;
        if (unitCurrentHealth <= 0) SetState(deadState);
    }

    public bool IsTargetInRange()
    {
        return target.currentPos.GetDistance(currentPos) < unitRange;
    }

    public ChessGrid FindNextGrid()
    {
        return ChessBoard.Instance.PathFinding(currentPos, target.currentPos);
    }

    public void SetState(IUnitState state)
    {
        currentState.Exit();
        currentState = state;
        currentState.Enter();
    }
}
