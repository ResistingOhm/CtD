using UnityEngine;

public class ShopUnitSlot: MonoBehaviour
{
    private UnitData slotData;

    public void SetSlotData(UnitData d)
    {
        slotData = d;
    }

    public int GetUnitID()
    {
        if (slotData != null)
            return slotData.unitID;

        return -1;
    }

    public int GetUnitCost()
    {
        if (slotData != null)
            return slotData.unitCost;

        return -1;
    }

    public void ClearSlot()
    {
        slotData = null;
        this.gameObject.SetActive(false);
    }
}
