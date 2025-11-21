using UnityEngine;

public class ShopItemSlot
{
    private ItemData slotData;

    public void SetSlotData(ItemData d)
    {
        slotData = d;
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
}
