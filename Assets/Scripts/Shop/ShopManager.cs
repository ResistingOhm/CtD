using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public Player player;

    public ShopUnitSlot[] unitSlots = new ShopUnitSlot[5];
    public ShopItemSlot[] itemSlots = new ShopItemSlot[3];

    private List<int> unitPool;

    private void Start()
    {
        unitPool = new List<int>();
        for (int i = 0; i < DataManager.unitData.Count; i++)
        {
            unitPool.Add(9);
        }
    }

    public void OnReroll()
    {
        if (player.gold < 2) return;
        player.gold -= 2;

        RerollShop();
    }

    public void RerollShop()
    {
        for (int i = 0; i < unitSlots.Length; i++)
        {
            UnitData unitData = GetRandomUnit();
            if (unitData != null)
            {
                unitSlots[i].SetSlotData(unitData);
            }
        }

        for (int i = 0; i < unitSlots.Length; i++)
        {
            ItemData itemData = DataManager.itemData[Random.Range(0, DataManager.itemData.Count)];
            if (itemData != null)
            {
                itemSlots[i].SetSlotData(itemData);
            }
        }
    }
    private UnitData GetRandomUnit()
    {
        // cost 및 확률에 따라 선택하는 로직은 생략 (기존 방식 사용)
        int cost = GetRandomCost();

        // 등장 가능한 유닛 후보만
        var candidates = DataManager.unitData.FindAll(u => u.unitCost == cost && unitPool[u.unitID] > 0);
        if (candidates.Count == 0)
            return null;

        UnitData selected = candidates[Random.Range(0, candidates.Count)];

        return selected;
    }

    private int GetRandomCost()
    {
        if (DataManager.shopData == null)
        {
            return 1;
        }

        // playerLevel 범위 보정: 배열은 level 1..N 순으로 들어있다고 가정
        int idx = Mathf.Clamp(player.GetDeckLevel() - 1, 0, DataManager.shopData.Count - 1);
        ShopProbabilityData probs = DataManager.shopData[idx];

        if (probs == null)
        {
            return 1;
        }

        // 랜덤 0..(sum-1)
        int r = Random.Range(0, 100);
        r += 1;
        int sum = 0;
        sum += Mathf.Max(0, probs.cost1);
        if (r < sum)
        {
            return 1;
        }
        sum += Mathf.Max(0, probs.cost2);
        if (r < sum)
        {
            return 2;
        }
        sum += Mathf.Max(0, probs.cost3);
        if (r < sum)
        {
            return 3;
        }
        sum += Mathf.Max(0, probs.cost4);
        if (r < sum)
        {
            return 4;
        }
        sum += Mathf.Max(0, probs.cost5);
        if (r < sum)
        {
            return 5;
        }

        return 1;

        
    }

    public void OnSelectUnitSlot(int slotNum)
    {
        var id = unitSlots[slotNum].GetUnitID();
        var cost = unitSlots[slotNum].GetUnitCost();
        if (id != -1)
        {
            if(player.AddUnit(id, 0, cost))
            {
                unitPool[id] -= 1;
            }
        }
    }

    public void OnSelectItemSlot(int slotNum)
    {
        var id = itemSlots[slotNum].GetItemID();
        var cost = itemSlots[slotNum].GetItemCost();
        if (id != -1)
        {
            player.AddItem(id, cost);
        }
    }
}
