using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class ResolutionController : MonoBehaviour
{
    private List<Resolution> resolutions = new List<Resolution>();

    private int currentResolution = 0;
    private int currentScreen = 0;

    [SerializeField]
    private TMP_Dropdown resolutionDropDown;

    void Awake()
    {
        foreach(Resolution r in Screen.resolutions)
        {
            if (r.width < 800 || (r.width * 9) != (r.height * 16)) continue;
            resolutions.Add(r);
        }
    }

    void Start()
    {
        resolutionDropDown.ClearOptions();

        int optionNum = 0;

        foreach(Resolution r in resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();

            option.text = r.width + " x " + r.height;
            resolutionDropDown.options.Add(option);

            if (r.width == Screen.width && r.height == Screen.height)
            {
                if (!PlayerPrefs.HasKey("Resolution"))
                {
                    resolutionDropDown.value = optionNum;
                    PlayerPrefs.SetInt("Resolution", optionNum);
                }
            }

            optionNum++;
        }

        if (!PlayerPrefs.HasKey("ScreenMode"))
        {
            PlayerPrefs.SetInt("ScreenMode", 2);
        }

        int re = PlayerPrefs.GetInt("Resolution");
        int sc = PlayerPrefs.GetInt("ScreenMode");

        if (re + 1 > optionNum)
        {
            re = optionNum - 1;
        }

        currentScreen = sc;
        resolutionDropDown.value = re;
    }

    public void ResolutionOptionChanged(int n)
    {
        currentResolution = n;
        PlayerPrefs.SetInt("Resolution", n);
        PlayerPrefs.Save();
        RefreshScreen();
    }

    public void ScreenOptionChanged(int n)
    {
        currentScreen = n;
        PlayerPrefs.SetInt("ScreenMode", n);
        PlayerPrefs.Save();
        RefreshScreen();
    }

    public void RefreshScreen()
    {
        switch(currentScreen)
        {
            case 0:
                Screen.SetResolution(resolutions[currentResolution].width, resolutions[currentResolution].height,true);
                break;
            case 2:
                Screen.SetResolution(resolutions[currentResolution].width, resolutions[currentResolution].height,FullScreenMode.Windowed);
                break;
        }
    }
}
