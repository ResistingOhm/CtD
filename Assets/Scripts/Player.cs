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

    public int gold;
    public int exp;
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
        84 
    };

    void Start()
    {
        deck = GetComponent<UnitDeck>();
    }

    public bool AddUnit(int id, int level, int cost)
    {
        if (cost > gold) return false;

        var g = ObjectPoolManager.Instance.SpawnFromPool<Unit>("Unit", Vector3.zero, Quaternion.identity);
        g.InitialSetting(DataManager.unitData[id], level, deck, true);
        units.Add(g);

        if (UnitLevelUp(g))
        {
            gold -= cost;
            return true;
        }

        for (int i = 0; i < 9; i++)
        {
            if (unitInventory[i].canAccept)
            {
                g.SetCurrentTile(unitInventory[i]);
                gold -= cost;
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
                gold -= cost;

                return true;
            }
        }

        return false;
    }

    public int AddEXP(int i)
    {
        if (gold < 4) return GetDeckLevel();
        if (deck.deckLevel >= 10) return GetDeckLevel();

        gold -= 4;
        exp += i;
        CheckLevelUp();

        return GetDeckLevel();
    }

    public void SellUnit(Unit u)
    {
        gold += u.GetUnitCost() * u.GetUnitLevel();
        var items = u.GetItems();

        for (int i = 0; i < items.Length; i++)
        {
            SellItem(items[i]);   
        }
    }

    public void SellItem(int i)
    {
        gold += DataManager.itemData[i].itemCost;
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
            lv++;

            // 최대 레벨 도달 시 EXP 초과분 제거
            if (lv >= 10)
            {
                exp = 0;
                break;
            }
        }

        deck.deckLevel = lv;
    }

    public int GetDeckLevel()
    {
        return deck.deckLevel;
    }

}
