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
    }

    private void LoadAudio()
    {
        bool isAudioOn = PlayerPrefs.GetInt(AudioKey, 1) == 1;

        audioToggle.isOn = isAudioOn;
        AudioListener.volume = isAudioOn ? 1f : 0f;
    }

    public void ChangeAudio(bool isAudioOn)
    {
        AudioListener.volume = isAudioOn ? 1f : 0f;

        PlayerPrefs.SetInt(AudioKey, isAudioOn ? 1 : 0);
        PlayerPrefs.Save();
    }
}