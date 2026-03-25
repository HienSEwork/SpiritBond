using UnityEngine;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    public Toggle audioToggle;

    private const string AudioKey = "audio";

    private void Start()
    {
        if (audioToggle == null)
            audioToggle = GetComponent<Toggle>();

        LoadAudio();
        Debug.Log($"[AudioSetting] Initialized. Toggle={(audioToggle != null)}");
    }

    private void LoadAudio()
    {
        bool isAudioOn = PlayerPrefs.GetInt(AudioKey, 1) == 1;

        audioToggle.isOn = isAudioOn;
        AudioListener.volume = isAudioOn ? 1f : 0f;
        Debug.Log($"[AudioSetting] Loaded audio setting: {isAudioOn}");
    }

    public void ChangeAudio(bool isAudioOn)
    {
        AudioListener.volume = isAudioOn ? 1f : 0f;

        PlayerPrefs.SetInt(AudioKey, isAudioOn ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log($"[AudioSetting] Audio changed: {isAudioOn}");
    }
}
