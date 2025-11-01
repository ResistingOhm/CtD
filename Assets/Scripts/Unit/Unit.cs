using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private UnitData unitData;
    [Range(0, 2)]
    private int level = 0;
    private Item[] items = new Item[3];
    private UnitDeck deck;
    private bool isDeck = false;


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
    private float unitAttackSpeed;

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
        idleState = new IdleState(this);
        attackingState = new AttackingState(this);
        movingState = new MovingState(this);
        deadState = new DeadState(this);
    }

    void Start()
    {
        GameManager.Instance.startFight += StartFighting;
        GameManager.Instance.endFight += EndFighting;
        GetComponent<DraggableObject>().dropAction += AfterDrop;
        SetState(idleState);
        RefreshStatus();
    }

    public void SetUnitData(UnitData u)
    {
        unitData = u;

    }

    public void StartFighting()
    {
        this.GetComponent<DraggableObject>().enabled = false;
        StartCoroutine(DoAction());
    }

    public void EndFighting()
    {
        this.GetComponent<DraggableObject>().enabled = true;
        SetState(idleState);
        RefreshStatus();
    }

    IEnumerator DoAction()
    {
        WaitForSeconds w = new WaitForSeconds(0.5f);
        while(GameManager.Instance.isRunning)
        {
            currentState.Update();
            yield return w;
        }
    }

    public void SetDeck(UnitDeck u)
    {
        deck = u;
    }

    public float GetAttackSpeed()
    {
        return unitAttackSpeed;
    }

    public int[] GetUnitSynergy()
    {
        return unitData.unitSynergy;
    }

    private void RefreshStatus()
    {
        //Apply Basic status
        unitMaxHealth = (int) (unitData.baseHealth * unitData.healthRate[level]);
        unitAttack = (int) (unitData.baseAttack * unitData.attackRate[level]);
        unitDefense = (int) (unitData.baseDefense * unitData.defenseRate[level]);
        unitAttackSpeed = unitData.baseAttackSpeed * unitData.attackSpeedRate[level];

        unitHealthRegen = unitData.baseHealthRegen;
        unitLifeSteal = unitData.baseLifeSteal;
        unitEvade = unitData.baseEvade;
        unitRange = unitData.baseRange;

        //Apply Synergy
        int[] s = GetUnitSynergy();
        for (int i = 0; i < s.Length; i++)
        {
            int sl = deck.unitSynergy[s[i]][1];

            unitMaxHealth += DataManager.unitSynergyData[s[i]].health[sl];
            unitAttack += DataManager.unitSynergyData[s[i]].attack[sl];
            unitDefense += DataManager.unitSynergyData[s[i]].defense[sl];
            unitAttackSpeed += DataManager.unitSynergyData[s[i]].attackSpeed[sl];

            unitHealthRegen += DataManager.unitSynergyData[s[i]].healthRegen[sl];
            unitLifeSteal += DataManager.unitSynergyData[s[i]].lifeSteal[sl];
            unitEvade += DataManager.unitSynergyData[s[i]].evade[sl];
            unitRange += DataManager.unitSynergyData[s[i]].range[sl];
        }

        //Apply Item
        for(int i = 0; i < 3; i++)
        {
            if (items[i] == null) break;
            ItemData item = items[i].itemData;
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

    private void AddItem(Item i)
    {
        for (int j = 0; j < 3; j++)
        {
            if (items[j] == null) items[j] = i;
            break;
        }
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

    public void AfterDrop(GameObject g)
    {
        this.transform.position = g.transform.position;
        if (!isDeck) deck.AddUnit(this);
    }
}
