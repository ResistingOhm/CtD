using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<Unit> units = new List<Unit>();

    [SerializeField]
    private DroppableTile[] unitInventory = new DroppableTile[9];
    [SerializeField]
    private DroppableTile[] itemInventory = new DroppableTile[9];
    [SerializeField]
    private PlayerUIManager uiManager;

    private int life = 3;

    public int gold = 0;
    public int exp = 0;
    private UnitDeck deck;

    public int[] expTable = new int[] {
        4, 
        6, 
        10,
        20,
        36,
        48,
        60,
        72,
        84,
        0,
    };

    void Start()
    {
        deck = GetComponent<UnitDeck>();
        uiManager.SetgoldText(gold);
        uiManager.SetLevelText(GetDeckLevel());
        uiManager.SetExpText(exp);
        uiManager.SetMaxExp(exp);
    }

    public bool AddUnit(int id, int level, int cost)
    {
        if (cost > gold) return false;

        var g = ObjectPoolManager.Instance.SpawnFromPool<Unit>("Unit", Vector3.zero, Quaternion.identity);
        g.InitialSetting(DataManager.unitData[id], level, deck, true);
        units.Add(g);

        if (UnitLevelUp(g))
        {
            GoldChange(-cost);
            return true;
        }

        for (int i = 0; i < 9; i++)
        {
            if (unitInventory[i].canAccept)
            {
                g.SetCurrentTile(unitInventory[i]);
                GoldChange(-cost);
                return true;
            }
        }
        RemoveUnit(g);

        return false;
    }

    public bool AddItem(int id, int cost)
    {
        if (cost > gold) return false;

        for (int i = 0; i < 9; i++)
        {
            if (itemInventory[i].canAccept)
            {
                var g = ObjectPoolManager.Instance.SpawnFromPool<Item>("Item", Vector3.zero, Quaternion.identity);
                g.InitialSetting(DataManager.itemData[id], itemInventory[i]);
                GoldChange(-cost);

                return true;
            }
        }

        return false;
    }

    public int BuyEXP(int i)
    {
        if (gold < 4) return GetDeckLevel();
        if (deck.deckLevel >= 10) return GetDeckLevel();

        GoldChange(-4);
        return AddEXP(i);
    }

    public int AddEXP(int i)
    {
        exp += i;
        CheckLevelUp();

        uiManager.SetExpText(exp);

        return GetDeckLevel();
    }

    public void SellUnit(Unit u)
    {
        GoldChange(u.GetUnitCost() * (u.GetUnitLevel() + 1));
        var items = u.GetItems();

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != -1)
                SellItem(items[i]);   
        }

        RemoveUnit(u);
    }

    public void SellItem(int i)
    {
        GoldChange(DataManager.itemData[i].itemCost);
    }

    public void SellItem(Item i)
    {
        GoldChange(i.GetItemCost());
        i.DeleteAll();
    }

    public void RemoveUnit(Unit u)
    {
        if (u.isDeck)
        {
            deck.RemoveUnitFromField(u);
        }

        units.Remove(u);
        u.DeleteAll();
    }

    public bool UnitLevelUp(Unit u)
    {
        int id = u.GetUnitID();
        int lv = u.GetUnitLevel();

        if (lv >= 2) return false;

        List<Unit> temp = new List<Unit>();
        temp.Clear();
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i].GetUnitID() == id && units[i].GetUnitLevel() == lv)
            {
                temp.Add(units[i]);
            }
        }

        if (temp.Count >= 3)
        {
            var f = temp[0];
            var s = temp[1];
            var l = temp[temp.Count-1];
            var sitems = s.GetItems();
            var litems = l.GetItems();
            f.LevelUp();

            for (int i = 0; i < sitems.Length; i++)
            {
                if (sitems[i] == -1) break;
                if (!f.AddItem(DataManager.itemData[sitems[i]]))
                {
                    SellItem(sitems[i]);
                }
            }

            for (int i = 0; i < litems.Length; i++)
            {
                if (litems[i] == -1) break;
                if (!f.AddItem(DataManager.itemData[litems[i]]))
                {
                    SellItem(litems[i]);
                }
            }

            RemoveUnit(l);
            RemoveUnit(s);

            UnitLevelUp(f);

            return true;
        }

        return false;
    }

    private void CheckLevelUp()
    {
        int lv = GetDeckLevel();
        while (lv < 10 && exp >= expTable[lv - 1])
        {
            exp -= expTable[lv - 1];
            lv += 1;
            uiManager.SetLevelText(lv);
            uiManager.SetMaxExp(expTable[lv - 1]);

            // 최대 레벨 도달 시 EXP 초과분 제거
            if (lv >= 10)
            {
                exp = 0;
                break;
            }
        }

        deck.deckLevel = lv;
    }

    public int DecreaseLife()
    {
        life -= 1;
        uiManager.DecreaseLife(life);
        return life;
    }

    public void GoldChange(int amount)
    {
        gold += amount;
        uiManager.SetgoldText(gold);
    }

    public int GetDeckLevel()
    {
        return deck.deckLevel;
    }

}
