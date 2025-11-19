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

    public int money = int.MaxValue;
    private UnitDeck deck;

    void Start()
    {
        deck = GetComponent<UnitDeck>();
    }

    public bool AddUnit(int id, int level, int cost)
    {
        if (cost > money) return false;

        if (UnitLevelUp(id, level))
        {
            money -= cost;
            return true;
        }

        for (int i = 0; i < 9; i++)
        {
            if (unitInventory[i].canAccept)
            {
                var g = ObjectPoolManager.Instance.SpawnFromPool<Unit>("Unit", Vector3.zero, Quaternion.identity);
                g.InitialSetting(DataManager.unitData[id], level, deck, true, unitInventory[i]);
                units.Add(g);
                money -= cost;

                return true;
            }
        }

        return false;
    }

    public bool AddItem(int id, int cost)
    {
        if (cost > money) return false;

        for (int i = 0; i < 9; i++)
        {
            if (itemInventory[i].canAccept)
            {
                var g = ObjectPoolManager.Instance.SpawnFromPool<Item>("Item", Vector3.zero, Quaternion.identity);
                g.InitialSetting(DataManager.itemData[id], itemInventory[i]);
                money -= cost;

                return true;
            }
        }

        return false;
    }

    public void SellUnit(Unit u)
    {
        money += u.GetUnitCost() * u.GetUnitLevel();
        var items = u.GetItems();

        for (int i = 0; i < items.Length; i++)
        {
            SellItem(items[i]);   
        }
    }

    public void SellItem(int i)
    {
        money += DataManager.itemData[i].itemCost;
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

    public bool UnitLevelUp(int id, int lv)
    {
        List<Unit> temp = new List<Unit>();
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i].GetUnitID() == id && units[i].GetUnitLevel() == lv)
            {
                temp.Add(units[i]);
            }
        }

        if (temp.Count >= 2)
        {
            var f = temp.First();
            var l = temp.Last();
            var items = l.GetItems();
            f.LevelUp();

            for (int i = 0; i < items.Length; i++)
            {
                if (!f.AddItem(DataManager.itemData[items[i]]))
                {
                    SellItem(items[i]);
                }
            }

            RemoveUnit(l);

            return true;
        }

        return false;
    }

    public List<Unit> GetDeckUnits()
    {
        return deck.units;
    }

}
