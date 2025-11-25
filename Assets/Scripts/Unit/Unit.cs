using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour
{
    private UnitData unitData;
    [Range(0, 2)]
    public int level = 0;
    private int[] items = {-1,-1,-1};
    private UnitDeck deck;
    public bool isDeck = false;

    public DroppableTile currentTile;
    public ChessGrid currentPos;
    public Unit target;

    private Collider2D unitCollider;
    private Rigidbody2D rb;
    private DraggableObject draggableObject;
    private UnitSprite unitSprite;
    private bool isAlly;

    [Header("-Status")]
    private UnitTotalStatus status;

    [Header("-State")]
    private IUnitState currentState;
    public IdleState idleState;
    public AttackingState attackingState;
    public MovingState movingState;
    public DeadState deadState;
    public WinningState winState;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        idleState = new IdleState(this);
        attackingState = new AttackingState(this);
        movingState = new MovingState(this);
        deadState = new DeadState(this);
        winState = new WinningState(this);

        draggableObject = GetComponent<DraggableObject>();
        draggableObject.dropAction += AfterDrop;
        draggableObject.changeAction += AfterChange;

        unitCollider = GetComponent<Collider2D>();
        unitSprite = GetComponent<UnitSprite>();
    }

     void OnEnable()
    {
        currentState = idleState;
    }

    public void InitialSetting(UnitData u, int lv, UnitDeck d, bool isAlly, DroppableTile tile = null)
    {
        unitData = u;
        level = lv;
        deck = d;

        gameObject.tag = isAlly ? "Ally" : "Enemy";
        this.isAlly = isAlly;

        if (tile  != null)
        {
            currentTile = tile;
            currentTile.NowFilled(draggableObject);
            this.transform.position = currentTile.transform.position;
        }

        status = new UnitTotalStatus();
        status.synergy = new Status[GetUnitSynergy().Length];
        UnitStatusSetting();

        unitSprite.InitialSetting(unitData.unitID, level, status.maxHealth);
    }

    public void SetCurrentTile(DroppableTile tile)
    {
        currentTile = tile;
        currentTile.NowFilled(draggableObject);
        this.transform.position = currentTile.transform.position;
    }

    public void DeleteAll()
    {
        unitData = null;
        level = 0;
        deck = null;
        gameObject.tag = "Untagged";
        isAlly = false;
        if (currentTile != null)
        {
            currentTile.NowEmpty();
            currentTile = null;
        }
        items[0] = -1;
        items[1] = -1;
        items[2] = -1;

        unitSprite.hpBar.gameObject.SetActive(false);

        this.gameObject.SetActive(false);
    }

    public void LevelUp()
    {
        level += 1;
        UnitStatusSetting();
        unitSprite.SetLevelSprite(level);
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
        currentPos = currentTile.gameObject.GetComponent<ChessGrid>();
        currentPos.cost = byte.MaxValue;
    }

    public void DoAction()
    {
        currentState.Update();
    }

    public void EndFighting()
    {
        draggableObject.enabled = true;
        unitCollider.enabled = true;
        SetState(idleState);
        target = null;

        if (currentPos != null)
        {
            currentPos.cost = 1;
            currentPos = null;
        }
        unitSprite.SetVisibility(true);

        this.transform.position = currentTile.transform.position;
        RefreshStatus();
    }

    public int GetUnitID()
    {
        return unitData.unitID;
    }

    public int GetUnitCost()
    {
        return unitData.unitCost;
    }

    public int GetUnitLevel()
    {
        return level;
    }

    public int[] GetItems()
    {
        return items;
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

        unitSprite.RefreshHp(status.maxHealth);
    }

    public void SetVelocity(float p)
    {
        Vector2 dir = currentPos.transform.position - transform.position;
        rb.linearVelocity = dir.normalized * p;
    }

    public void SetTarget()
    {
        System.Collections.Generic.List<Unit> enemy;
        if (isAlly) enemy = GameManager.Instance.enemyDeck.units;
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
        int i = Mathf.CeilToInt(damage - (damage * status.defense / 100));
        if (i > 0) status.unitCurrentHealth -= i;
        unitSprite.SetCurrentHp(status.unitCurrentHealth);
        if (status.unitCurrentHealth <= 0) SetState(deadState);

        return i;
    }

    public void UnitDeadAction()
    {
        unitCollider.enabled = false;
        SetVelocity(0);
        currentPos.cost = 1;

        GameManager.Instance.UnitDeadCounter(isAlly);
    }

    public void UnitDisableAfterDeath()
    {
        currentPos = null;
        unitSprite.SetVisibility(false);
    }

    public void GainHealth(int n)
    {
        status.unitCurrentHealth += n;
        if (status.unitCurrentHealth > status.maxHealth) status.unitCurrentHealth = status.maxHealth;
        unitSprite.SetCurrentHp(status.unitCurrentHealth);
    }

    public bool AddItem(ItemData i)
    {
        for (int j = 0; j < 3; j++)
        {
            if (items[j] == -1)
            {
                items[j] = i.itemID;
                status.items[j] = Status.Converter(i);

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
        if (g.CompareTag("Board"))
        {
            if (!isDeck)
            {
                if (!deck.IsAbleToAddUnitToField())
                {
                    draggableObject.wasDroppedOnValidSlot = false;
                    return;
                }

                deck.AddUnitToField(this);
                RefreshStatus();
            }
        }

        this.transform.position = g.transform.position;

        if (currentTile != null && currentTile.objectNow == draggableObject)
        {
            currentTile.NowEmpty();
            currentTile = null;
        }

        currentTile = g.GetComponent<DroppableTile>();
        currentTile.NowFilled(draggableObject);


        if (g.CompareTag("Inventory"))
        {
            if (isDeck)
            {
                deck.RemoveUnitFromField(this);
                RefreshStatus();
            }
        }
    }

    public void AfterChange(DroppableTile droppedTile, DroppableTile previousTile)
    {
        droppedTile.objectNow.GetComponent<Unit>().AfterDrop(previousTile.gameObject);
        AfterDrop(droppedTile.gameObject);
    }
}
