using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SynergyUISlot : MonoBehaviour
{
    [SerializeField]
    private Image synergySprite;
    [SerializeField]
    private TextMeshProUGUI numText;
    [SerializeField]
    private TextMeshProUGUI nameText;

    public int synergyId;
    public int synergyNum;
    public int synergyLv;

    public void InitialSetting(int id)
    {
        synergyId = id;
        synergyNum = 0;
        synergyLv = 0;

        SetSprite();
    }

    public void SetData(int[] data)
    {
        synergyNum = data[0];
        synergyLv = data[1];

        SetLevel();
        SetNum();
    }

    public void UpdateUI()
    {
        gameObject.SetActive(true);
        SetLevel();
        SetNum();
    }

    private void SetSprite()
    {
        //Set Sprite
        nameText.text = DataManager.unitSynergyData[synergyId].synergyName;
    }

    private void SetLevel()
    {
        switch (synergyLv)
        {
            case 0:
                synergySprite.color = Color.black;
                break;
            case 1:
                synergySprite.color = Color.blue;
                break;
            case 2:
                synergySprite.color = Color.gray;
                break;
            case 3:
                synergySprite.color = Color.yellow;
                break;
            case 4:
                synergySprite.color = Color.white;
                break;
            default:
                synergySprite.color = Color.green;
                break;

        }
    }

    private void SetNum()
    {
        numText.text = synergyNum.ToString();
    }
}
