using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;

    [Header("CanvasGroup for Fade UI")]
    public CanvasGroup fadeCanvasGroup;

    [Header("Fade Time")]
    public float fadeDuration = 0.8f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 넘어가도 유지
        }
        else
        {
            Destroy(gameObject);
        }

        fadeCanvasGroup.alpha = 0f;
        fadeCanvasGroup.gameObject.SetActive(false);
    }

    public void FadeOut(Action action)
    {
        fadeCanvasGroup.gameObject.SetActive(true);

        fadeCanvasGroup.DOFade(1f, fadeDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                action?.Invoke();
                FadeInFromBlack();
            });
    }

    // 씬 로드 후 페이드 인
    private void FadeInFromBlack()
    {
        fadeCanvasGroup.DOFade(0f, fadeDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                fadeCanvasGroup.gameObject.SetActive(false); // 완료 후 비활성 처리
            });
    }
}
