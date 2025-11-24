using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Player player;

    public ShopUnitSlot[] unitSlots = new ShopUnitSlot[5];
    public ShopItemSlot[] itemSlots = new ShopItemSlot[3];

    public GameObject sellPanel;

    private List<int> unitPool;
    private int[] probabilityPool = { 1, 1, 1, 1, 1 };

    private void Start()
    {
        unitPool = new List<int>();
        for (int i = 0; i < DataManager.unitData.Count; i++)
        {
            unitPool.Add(9);
        }

        var candidates = DataManager.unitData.FindAll(u => u.unitCost == 1 && unitPool[u.unitID] > 0);
        if (candidates.Count == 0)
            return;

        for (int i = 0; i < unitSlots.Length; i++)
        {
            UnitData unitData = candidates[Random.Range(0, candidates.Count)];
            if (unitData != null)
            {
                unitSlots[i].SetSlotData(unitData);
            }
        }

        for (int i = 0; i < itemSlots.Length; i++)
        {
            ItemData itemData = DataManager.itemData[Random.Range(0, DataManager.itemData.Count)];
            if (itemData != null)
            {
                itemSlots[i].SetSlotData(itemData);
            }
        }
    }

    public void OnReroll()
    {
        if (player.gold < 2) return;
        player.GoldChange(-2);

        RerollShop();
    }

    public void RerollShop()
    {
        for (int i = 0; i < unitSlots.Length; i++)
        {
            unitSlots[i].ClearSlot();

            UnitData unitData = GetRandomUnit();
            if (unitData != null)
            {
                unitSlots[i].SetSlotData(unitData);
                unitSlots[i].gameObject.SetActive(true);
            }
        }

        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].ClearSlot();

            ItemData itemData = DataManager.itemData[Random.Range(0, DataManager.itemData.Count)];
            if (itemData != null)
            {
                itemSlots[i].SetSlotData(itemData);
                itemSlots[i].gameObject.SetActive(true);
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

        int sum = 0;
        for (int i = 0; i < 5; i++) sum += Mathf.Max(0, probs.GetProbability(i));

        int r = Random.Range(0, sum);
        r += 1;
        int cumulative = 0;
        for (int i = 0; i < 5; i++)
        {
            cumulative += Mathf.Max(0, probs.GetProbability(i) * probabilityPool[i]);
            if (r < cumulative)
            {
                return i + 1; // index 0 -> 1 cost
            }
        }

        return 1;
        
    }

    public void OnclickExpButton(Button button)
    {
        int i = player.AddEXP(4);
        if (i >= 10) button.interactable = false;
    }

    private void RemoveFromPool(int id, int cost)
    {
        unitPool[id] -= 1;
        var candidates = DataManager.unitData.FindAll(u => u.unitCost == cost && unitPool[u.unitID] > 0);

        if (candidates.Count > 0) return;

        probabilityPool[cost - 1] = 0;
    }

    private void AddToPool(int id)
    {
        unitPool[id] += 1;
        if (unitPool[id] > 0)
        {
            probabilityPool[id] = 1;
        }
    }

    public void OnSelectUnitSlot(int slotNum)
    {
        var slot = unitSlots[slotNum];
        var id = slot.GetUnitID();
        var cost = slot.GetUnitCost();
        if (id != -1)
        {
            if(player.AddUnit(id, 0, cost))
            {
                RemoveFromPool(id, cost);
                slot.ClearSlot();
            }
        }
    }

    public void OnSelectItemSlot(int slotNum)
    {
        var slot = itemSlots[slotNum];
        var id = slot.GetItemID();
        var cost = slot.GetItemCost();
        if (id != -1)
        {
            player.AddItem(id, cost);
            slot.ClearSlot();
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            sellPanel.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        sellPanel.SetActive(false);
    }

    public void OnDrop(PointerEventData eventData)
    {
        sellPanel.SetActive(false);

        var t = eventData.pointerDrag.GetComponent<DroppableTile>();
        if (!t.canDrag) return;
        var g = t.objectNow;
        if (g == null) return;

        g.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        g.gameObject.layer = 11;

        var gt = g.tag;

        if (gt.Equals("Item"))
        {
            player.SellItem(g.GetComponent<Item>());
            return;
        }

        if (gt.Equals("Ally"))
        {
            var a = g.GetComponent<Unit>();
            AddToPool(a.GetUnitID());
            player.SellUnit(a);
            return;
        }
    }
}
