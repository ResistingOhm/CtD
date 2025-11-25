using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUnitSlotUIManager : MonoBehaviour
{
    [SerializeField]
    private Image unitSprite;
    [SerializeField]
    private TextMeshProUGUI cost;
    [SerializeField]
    private TextMeshProUGUI unitName;
    [SerializeField]
    private Image[] synergySprite;
    [SerializeField]
    private TextMeshProUGUI[] synergy;


    public void SetSlotData(UnitData u)
    {
        for (int i = 0; i < synergy.Length; i++)
        {
            synergySprite[i].gameObject.SetActive(false);
            synergy[i].gameObject.SetActive(false);
        }
        int unitId = u.unitID;
        int[] synergyId = u.unitSynergy;

        cost.text = u.unitCost.ToString();
        unitName.text = u.unitName;

        //Set unitSprite

        for (int i = 0; i < synergyId.Length; i++)
        {
            synergySprite[i].gameObject.SetActive(true);
            synergy[i].gameObject.SetActive(true);

            //Set synergySprite
            synergy[i].text = DataManager.unitSynergyData[synergyId[i]].synergyName;
        }
    }
}
