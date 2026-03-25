using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonUI : MonoBehaviour
{
    public string targetSceneName = "Map Forest";
    public string loadingSceneName = "LoadingScene";

    public void PlayGame()
    {
        Debug.Log($"[PlayButtonUI] PlayGame clicked. Target={targetSceneName} LoadingScene={loadingSceneName}");
        SceneLoaderData.nextSceneName = targetSceneName;
        SceneManager.LoadScene(loadingSceneName);
    }
}
