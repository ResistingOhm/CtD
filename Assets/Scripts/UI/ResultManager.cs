using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    int isWin;

    [SerializeField]
    private Image resultImage;
    [SerializeField]
    private TextMeshProUGUI resultText;

    public Image winImage;
    public Image loseImage;
    public Image errorImage;

    public string winText;
    public string loseText;
    public string errorText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!PlayerPrefs.HasKey("isWin"))
        {
            PlayerPrefs.SetInt("MasterVolume", -1);
        }

        isWin = PlayerPrefs.GetInt("isWin");

        switch (isWin)
        {
            case 0:
                resultImage = winImage;
                resultText.text = winText;
                break;
            case 1:
                resultImage = loseImage;
                resultText.text = loseText;
                break;
            default:
                resultImage = errorImage;
                resultText.text = errorText;
                break;
        }
    }

    public void OnClickToMenu()
    {
        FadeManager.Instance.FadeOut(() =>
        {
            SceneManager.LoadScene(0);
        });
    }
}
