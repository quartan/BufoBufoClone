using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

[Serializable]
public class ScreenResolution
{
    public int widthScreen = 1920;
    public int heightScreen = 1080;
}

[RequireComponent(typeof(Dropdown))]
public class ScreenResolutionController : MonoBehaviour
{
    private Dropdown dropdown;
    [SerializeField] private List<ScreenResolution> screenResolutions;

    private void Start()
    {
        
        dropdown = GetComponent<Dropdown>();
        dropdown.ClearOptions();
        List<string> textOptions = new List<string>();
        for (int i = 0; i < screenResolutions.Count; i++)
        {
            textOptions.Add($"{screenResolutions[i].widthScreen}*{screenResolutions[i].heightScreen}");
        }
        dropdown.AddOptions(textOptions);
        dropdown.onValueChanged.RemoveAllListeners();
        dropdown.onValueChanged.AddListener((value) =>
        {
            Screen.SetResolution(screenResolutions[value].widthScreen, screenResolutions[value].heightScreen, true);
            PlayerPrefs.SetInt("ScreenResolution", value);
        });

        if(PlayerPrefs.HasKey("ScreenResolution"))
        {
            int index = PlayerPrefs.GetInt("ScreenResolution", 0);
            if(index >= 0 && index <= screenResolutions.Count)
            {
                Screen.SetResolution(screenResolutions[index].widthScreen, screenResolutions[index].heightScreen, true);
                dropdown.value = index;
            }
            else
            {
                Screen.SetResolution(1920, 1080, true);
                dropdown.value = 0;
            }
        }
        else
        {
            Screen.SetResolution(1920, 1080, true);
            dropdown.value = 0;
        }
    }
}
