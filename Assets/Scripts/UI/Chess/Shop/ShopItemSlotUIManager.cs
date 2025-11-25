using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemSlotUIManager : MonoBehaviour
{
    [SerializeField]
    private Image itemSprite;
    [SerializeField]
    private TextMeshProUGUI cost;
    [SerializeField]
    private TextMeshProUGUI itemName;


    public void SetSlotData(ItemData i)
    {
        int itemId = i.itemID;

        cost.text = i.itemCost.ToString();
        itemName.text = i.itemName;

        //Set itemSprite
    }
}
