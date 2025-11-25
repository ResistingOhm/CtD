using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public RectTransform panel;
    public TextMeshProUGUI panelText; // UI Text 연결 (TMP인 경우 TMP_Text로 변경)

    public Vector2 startPosRight;
    public Vector2 centerPos;
    public Vector2 endPosLeft;

    public float moveDuration = 0.5f;
    public float stayDuration = 2f;

    // 외부에서 텍스트 넣도록 파라미터 추가
    public IEnumerator PlayPanelAnimation(string message)
    {
        panelText.text = message;

        panel.gameObject.SetActive(true);
        panel.anchoredPosition = startPosRight;

        yield return panel.DOAnchorPos(centerPos, moveDuration)
                         .SetEase(Ease.OutBack)
                         .WaitForCompletion();

        yield return new WaitForSeconds(stayDuration);

        yield return panel.DOAnchorPos(endPosLeft, moveDuration)
                         .SetEase(Ease.InBack)
                         .WaitForCompletion();

        panel.gameObject.SetActive(false);
    }
}
