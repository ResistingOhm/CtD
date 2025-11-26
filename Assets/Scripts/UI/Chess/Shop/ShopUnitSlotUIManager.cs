using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUnitSlotUIManager : MonoBehaviour
{
    [SerializeField]
    private Image unitSprite;
    [SerializeField]
    private Image costSprite;

    public Sprite[] costSprites;

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

        int c = u.unitCost;
        cost.text = c.ToString();

        switch (c)
        {
            case 1:
                costSprite.sprite = costSprites[0];
                costSprite.color = Color.white;
                break;
            case 2:
                costSprite.sprite = costSprites[1];
                costSprite.color = Color.white;
                break;
            case 3:
                costSprite.sprite = costSprites[2];
                costSprite.color = Color.white;
                break;
            case 4:
                costSprite.sprite = costSprites[3];
                costSprite.color = Color.white;
                break;
            case 5:
                costSprite.sprite = costSprites[3];
                costSprite.color = Color.red;
                break;
            default:
                costSprite.sprite = costSprites[0];
                costSprite.color = Color.white;
                break;
        }

        unitName.text = u.unitName;

        unitSprite.sprite = DataManager.unitSpriteData[unitId][0];

        for (int i = 0; i < synergyId.Length; i++)
        {
            synergySprite[i].gameObject.SetActive(true);
            synergy[i].gameObject.SetActive(true);

            //Set synergySprite
            synergy[i].text = DataManager.unitSynergyData[synergyId[i]].synergyName;
        }
    }
}
