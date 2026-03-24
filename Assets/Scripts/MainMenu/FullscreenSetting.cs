using UnityEngine;
using UnityEngine.UI;

public class FullscreenSetting : MonoBehaviour
{
    public Toggle fullscreenToggle;

    private const string FullscreenKey = "fullscreen";
    private const string ResolutionKey = "resolution_index";

    private readonly Vector2Int[] resolutions =
    {
        new Vector2Int(1920, 1080),
        new Vector2Int(1600, 900),
        new Vector2Int(1280, 720)
    };

    private void Start()
    {
        if (fullscreenToggle == null)
            fullscreenToggle = GetComponent<Toggle>();

        LoadFullscreen();
    }

    private void LoadFullscreen()
    {
        bool isFullscreen = PlayerPrefs.GetInt(FullscreenKey, 0) == 1;

        fullscreenToggle.SetIsOnWithoutNotify(isFullscreen);
        ApplyFullscreen(isFullscreen);
    }

    public void ChangeFullscreen(bool isFullscreen)
    {
        PlayerPrefs.SetInt(FullscreenKey, isFullscreen ? 1 : 0);
        PlayerPrefs.Save();

        ApplyFullscreen(isFullscreen);
    }

    private void ApplyFullscreen(bool isFullscreen)
    {
        int savedIndex = PlayerPrefs.GetInt(ResolutionKey, 0);
        savedIndex = Mathf.Clamp(savedIndex, 0, resolutions.Length - 1);

        int width = resolutions[savedIndex].x;
        int height = resolutions[savedIndex].y;

        if (isFullscreen)
        {
            Screen.SetResolution(width, height, FullScreenMode.ExclusiveFullScreen);
        }
        else
        {
            Screen.SetResolution(width, height, FullScreenMode.Windowed);
        }

        Debug.Log("Fullscreen applied: " + isFullscreen + " | Resolution: " + width + "x" + height);
    }
}