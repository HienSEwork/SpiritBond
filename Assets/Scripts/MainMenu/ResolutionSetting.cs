using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ResolutionSetting : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;

    private List<Vector2Int> resolutions = new List<Vector2Int>()
    {
        new Vector2Int(1920, 1080),
        new Vector2Int(1600, 900),
        new Vector2Int(1280, 720)
    };

    private const string ResolutionKey = "resolution_index";
    private const string FullscreenKey = "fullscreen";

    private void Start()
    {
        if (resolutionDropdown == null)
            resolutionDropdown = GetComponent<TMP_Dropdown>();

        SetupDropdown();
        LoadResolution();
    }

    private void SetupDropdown()
    {
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        foreach (Vector2Int res in resolutions)
        {
            options.Add(res.x + " x " + res.y);
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.RefreshShownValue();
    }

    private void LoadResolution()
    {
        int savedIndex = PlayerPrefs.GetInt(ResolutionKey, 0);
        savedIndex = Mathf.Clamp(savedIndex, 0, resolutions.Count - 1);

        resolutionDropdown.SetValueWithoutNotify(savedIndex);
        resolutionDropdown.RefreshShownValue();

        ApplyResolution(savedIndex);
    }

    public void ChangeResolution(int index)
    {
        PlayerPrefs.SetInt(ResolutionKey, index);
        PlayerPrefs.Save();

        ApplyResolution(index);
    }

    private void ApplyResolution(int index)
    {
        index = Mathf.Clamp(index, 0, resolutions.Count - 1);

        int width = resolutions[index].x;
        int height = resolutions[index].y;

        bool isFullscreen = PlayerPrefs.GetInt(FullscreenKey, 0) == 1;

        if (isFullscreen)
        {
            Screen.SetResolution(width, height, FullScreenMode.ExclusiveFullScreen);
        }
        else
        {
            Screen.SetResolution(width, height, FullScreenMode.Windowed);
        }

        Debug.Log("Resolution applied: " + width + "x" + height + " | Fullscreen: " + isFullscreen);
    }
}