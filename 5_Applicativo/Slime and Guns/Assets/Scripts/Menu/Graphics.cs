using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Graphics : MonoBehaviour
{
    public TMP_Dropdown ResDropDown;
    public Toggle FullScreenToggle;

    Resolution[] AllResolutions;
    List<Resolution> SelectedResolutionList = new List<Resolution>();

    int SelectedResolution;
    bool IsFullScreen;

    void Start()
    {
        AllResolutions = Screen.resolutions;
        IsFullScreen = Screen.fullScreen;

        List<string> resolutionStringList = new List<string>();

        foreach (Resolution res in AllResolutions)
        {
            string newRes = res.width + " x " + res.height;

            if (!resolutionStringList.Contains(newRes))
            {
                resolutionStringList.Add(newRes);
                SelectedResolutionList.Add(res);
            }
        }

        ResDropDown.ClearOptions();
        ResDropDown.AddOptions(resolutionStringList);

        FullScreenToggle.isOn = Screen.fullScreen;
    }

    public void ChangeResolution()
    {
        SelectedResolution = ResDropDown.value;

        Screen.SetResolution(
            SelectedResolutionList[SelectedResolution].width,
            SelectedResolutionList[SelectedResolution].height,
            IsFullScreen
        );
    }

    public void ChangeFullScreen()
    {
        IsFullScreen = FullScreenToggle.isOn;

        Screen.SetResolution(
            SelectedResolutionList[SelectedResolution].width,
            SelectedResolutionList[SelectedResolution].height,
            IsFullScreen
        );
    }
}
