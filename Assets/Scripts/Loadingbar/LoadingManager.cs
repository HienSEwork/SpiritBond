using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    [Header("UI")]
    public Slider loadingBar;
    public TMP_Text percentText;
    public TMP_Text loadingText;

    [Header("Fake Delay")]
    public float minLoadingTime = 1.0f;

    private void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    private IEnumerator LoadSceneAsync()
    {
        string sceneToLoad = SceneLoaderData.nextSceneName;

        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogError("SceneLoaderData.nextSceneName is empty!");
            yield break;
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        operation.allowSceneActivation = false;

        float timer = 0f;

        while (!operation.isDone)
        {
            timer += Time.deltaTime;

            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            float displayProgress = Mathf.Clamp01(Mathf.Min(progress, timer / minLoadingTime));

            if (loadingBar != null)
                loadingBar.value = displayProgress;

            if (percentText != null)
                percentText.text = Mathf.RoundToInt(displayProgress * 100f) + "%";

            if (loadingText != null)
                loadingText.text = "Loading...";

            if (operation.progress >= 0.9f && timer >= minLoadingTime)
            {
                if (loadingBar != null)
                    loadingBar.value = 1f;

                if (percentText != null)
                    percentText.text = "100%";

                yield return new WaitForSeconds(0.2f);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}