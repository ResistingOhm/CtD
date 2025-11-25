using UnityEngine;

public class ShopItemSlot: MonoBehaviour
{
    private ItemData slotData;
    private ShopItemSlotUIManager uiManager;

    private void Awake()
    {
        uiManager = GetComponent<ShopItemSlotUIManager>();
    }
    public void SetSlotData(ItemData d)
    {
        slotData = d;
        uiManager.SetSlotData(d);
    }

    public int GetItemID()
    {
        if (slotData != null)
            return slotData.itemID;

        return -1;
    }

    public int GetItemCost()
    {
        if (slotData != null)
            return slotData.itemCost;

        return -1;
    }

    public void ClearSlot()
    {
        slotData = null;
        this.gameObject.SetActive(false);
    }
}
