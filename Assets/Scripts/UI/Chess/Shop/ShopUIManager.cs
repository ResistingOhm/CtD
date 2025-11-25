using DG.Tweening;
using System.Collections;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ShopUIManager : MonoBehaviour
{
    [SerializeField]
    private RectTransform shopPanel;

    [SerializeField]
    private RectTransform toggleButtonText;

    private bool isOn = false;

    public float moveDuration = 0.5f;


    public void toggleShop()
    {

        isOn = !isOn;

        if (isOn)
        {
            StartCoroutine(PanelAnimation(1));
            toggleButtonText.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            StartCoroutine(PanelAnimation(0));
            toggleButtonText.eulerAngles = new Vector3(0, 0, 180);
        }
    }

    private IEnumerator PanelAnimation(float n)
    {
        yield return shopPanel.DOPivotY(n, moveDuration)
                         .SetEase(Ease.OutQuint)
                         .WaitForCompletion();
    }
}
