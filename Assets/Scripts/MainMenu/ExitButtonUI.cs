using UnityEngine;

public class ExitButtonUI : MonoBehaviour
{
    public void ExitGame()
    {
        Debug.Log("[ExitButtonUI] ExitGame clicked.");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
