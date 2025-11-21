using TMPro;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    public static SettingManager Instance { get; private set; }

    [SerializeField]
    private GameObject settingPanel;

    [SerializeField]
    private TextMeshProUGUI titleText;

    [SerializeField]
    private GameObject pausePanel;

    [SerializeField]
    private GameObject resolutionPanel;

    [SerializeField]
    private GameObject audioPanel;

    [SerializeField]
    private GameObject quitPanel;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PressEsc();
        }
    }

    public void PressEsc()
    {
        if (settingPanel.activeInHierarchy)
        {
            if (!pausePanel.activeInHierarchy)
            {
                BackToPause();
            }
            else
            {
                settingPanel.SetActive(false);
            }
        }
        else
        {
            settingPanel.SetActive(true);
        }
    }

    public void BackToPause()
    {
        pausePanel.SetActive(true);

        resolutionPanel.SetActive(false);
        audioPanel.SetActive(false);
        quitPanel.SetActive(false);

        titleText.text = "Title";
    }

    public void QuitTheGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
}
