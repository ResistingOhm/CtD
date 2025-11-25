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

    public Sprite winImage;
    public Sprite loseImage;
    public Sprite errorImage;

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
                resultImage.sprite = winImage;
                resultText.text = winText;
                break;
            case 1:
                resultImage.sprite = loseImage;
                resultText.text = loseText;
                break;
            default:
                resultImage.sprite = errorImage;
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
