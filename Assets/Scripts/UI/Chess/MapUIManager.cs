using DG.Tweening;
using System.Collections;
using UnityEngine;

public class MapUIManager : MonoBehaviour
{
    [SerializeField]
    private RectTransform mapPanel;

    public Vector2 startPos;
    public Vector2 centerPos;

    public float moveDuration = 0.5f;

    public void ToggleMap(bool isOn)
    {
        if (isOn)
        {
            StartCoroutine(PanelAnimation(centerPos));
        }
        else
        {
            StartCoroutine(PanelAnimation(startPos));
        }
    }

    private IEnumerator PanelAnimation(Vector2 v)
    {
        yield return mapPanel.DOAnchorPos(v, moveDuration)
                         .SetEase(Ease.OutQuint)
                         .WaitForCompletion();
    }
}
