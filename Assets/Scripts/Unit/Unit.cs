using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour
{
    private UnitData unitData;
    [Range(0, 2)]
    private int level = 0;
    private Item[] items = new Item[3];
    private UnitDeck deck;
    private bool isDeck = false;

    public ChessGrid currentPos;
    public Unit target;

    private Rigidbody2D rb;
    private DraggableObject draggableObject;

    [Header("-Status")]
    private UnitTotalStatus status;

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

        draggableObject = GetComponent<DraggableObject>();
        draggableObject.dropAction += AfterDrop;
        draggableObject.changeAction += AfterChange;
    }

     void OnEnable()
    {
        currentState = idleState;
    }

    public void InitialSetting(UnitData u, UnitDeck d, bool isAlly)
    {
        unitData = u;
        deck = d;

        gameObject.tag = isAlly ? "Ally" : "Enemy";


        status = new UnitTotalStatus();
        status.synergy = new Status[GetUnitSynergy().Length];
        UnitStatusSetting();
    }

    private void UnitStatusSetting()
    {
        status.unit = Status.Converter(unitData, level);
    }

    private void SynergyStatusSetting()
    {
        int[] s = GetUnitSynergy();
        for (int i = 0; i < s.Length; i++)
        {
            int sl = deck.unitSynergy[s[i]][1] - 1;

            if (sl < 0)
            {
                status.synergy[i] = new Status();
                continue;
            }
            status.synergy[i] = Status.Converter(DataManager.unitSynergyData[s[i]],sl);
        }
    }

    public void StartFighting()
    {
        draggableObject.enabled = false;
    }

    public void DoAction()
    {
        currentState.Update();
    }

    public void EndFighting()
    {
        draggableObject.enabled = true;
        SetState(idleState);
        RefreshStatus();
    }


    public float GetAttackSpeed()
    {
        return status.attackSpeed;
    }

    public int GetAttack()
    {
        return status.attack;
    }

    public int GetLifeSteal()
    {
        return status.lifeSteal;
    }

    public int[] GetUnitSynergy()
    {
        return unitData.unitSynergy;
    }

    public int GetCurrentHealth()
    {
        return status.unitCurrentHealth;
    }

    public void RefreshStatus()
    {
        UnitStatusSetting();
        if (isDeck) SynergyStatusSetting();
        status.RefreshTotalStatus(isDeck);
    }

    public void SetVelocity(float p)
    {
        Vector2 dir = currentPos.transform.position - transform.position;
        rb.linearVelocity = dir.normalized * p;
    }

    public void SetTarget()
    {
        System.Collections.Generic.List<Unit> enemy;
        if (this.CompareTag("Ally")) enemy = GameManager.Instance.enemyDeck.units;
        else enemy = GameManager.Instance.playerDeck.units;

        int min = 255;

        foreach(var e in enemy)
        {
            if (e.status.unitCurrentHealth <= 0) continue;
            var n = e.currentPos.GetDistance(currentPos);
            if (n < min)
            {
                min = n;
                target = e;
            }
        }
    }

    public int GetDamage(int damage)
    {
        int i = damage - damage * status.defense / 100;
        if (i > 0) status.unitCurrentHealth -= i;
        if (status.unitCurrentHealth <= 0) SetState(deadState);

        return i;
    }

    public void GainHealth(int n)
    {
        status.unitCurrentHealth += n;
        if (status.unitCurrentHealth > status.maxHealth) status.unitCurrentHealth = status.maxHealth;
    }

    public bool AddItem(Item i)
    {
        for (int j = 0; j < 3; j++)
        {
            if (items[j] == null)
            {
                items[j] = i;
                status.items[j] = Status.Converter(i.itemData);

                return true;
            }
        }

        return false;
    }

    public bool IsTargetInRange()
    {
        return target.currentPos.GetDistance(currentPos) <= status.range;
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

        if (currentPos != null)
        {
            currentPos.NowEmpty();
            currentPos = null;
        }

        if (g.CompareTag("Board"))
        {
            currentPos = g.GetComponent<ChessGrid>();
            currentPos.NowFilled(draggableObject);
            if (!isDeck)
            {
                deck.AddUnit(this);
                isDeck = true;
                RefreshStatus();
            }
        } else if (g.CompareTag("Inventory"))
        {
            if (isDeck)
            {
                deck.RemoveUnit(this);
                isDeck = false;
                RefreshStatus();
            }
        }
    }

    public void AfterChange(GameObject droppedTile, GameObject previousTile)
    {
        if (isDeck)
        {
            AfterDrop(droppedTile);
            droppedTile.GetComponent<Unit>().AfterDrop(previousTile);
        } else
        {
            droppedTile.GetComponent<Unit>().AfterDrop(previousTile);
            AfterDrop(droppedTile);
        }

    }
}
