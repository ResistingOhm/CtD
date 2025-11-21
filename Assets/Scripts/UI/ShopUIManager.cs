using TMPro;
using UnityEngine;

public class ShopUIManager : MonoBehaviour
{
    [SerializeField]
    private RectTransform shopPanel;

    [SerializeField]
    private RectTransform toggleButtonText;

    private bool isOn = false;

    public void toggleShop()
    {

        isOn = !isOn;

        if (isOn)
        {
            shopPanel.pivot = new Vector2(0.5f, 1);
            toggleButtonText.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            shopPanel.pivot = new Vector2(0.5f, 0);
            toggleButtonText.eulerAngles = new Vector3(0, 0, 180);
        }
    }
}
