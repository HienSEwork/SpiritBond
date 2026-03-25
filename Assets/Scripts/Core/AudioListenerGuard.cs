using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpiritBond.Core
{
    public static class AudioListenerGuard
    {
        private static int lastProcessedFrame = -1;
        private static string lastProcessedSceneName = string.Empty;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            EnsureSingleActiveAudioListener(scene);
        }

        public static void EnsureSingleActiveAudioListener(Scene activeScene)
        {
            if (lastProcessedFrame == Time.frameCount && string.Equals(lastProcessedSceneName, activeScene.name, System.StringComparison.Ordinal))
            {
                return;
            }

            AudioListener[] listeners = Object.FindObjectsByType<AudioListener>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            if (listeners == null || listeners.Length <= 1)
            {
                lastProcessedFrame = Time.frameCount;
                lastProcessedSceneName = activeScene.name;
                return;
            }

            AudioListener preferredListener = FindPreferredListener(listeners, activeScene);
            bool preferredEnabled = false;

            for (int i = 0; i < listeners.Length; i++)
            {
                AudioListener listener = listeners[i];
                if (listener == null)
                {
                    continue;
                }

                bool shouldEnable = listener == preferredListener && !preferredEnabled;
                listener.enabled = shouldEnable;
                preferredEnabled |= shouldEnable;
            }

            lastProcessedFrame = Time.frameCount;
            lastProcessedSceneName = activeScene.name;
            Debug.Log($"[AudioListenerGuard] Active scene={activeScene.name} | listeners={listeners.Length} | kept={preferredListener.gameObject.name}");
        }

        private static AudioListener FindPreferredListener(IReadOnlyList<AudioListener> listeners, Scene activeScene)
        {
            Camera sceneMainCamera = Camera.main;
            if (sceneMainCamera != null && sceneMainCamera.gameObject.scene == activeScene)
            {
                AudioListener mainCameraListener = sceneMainCamera.GetComponent<AudioListener>();
                if (mainCameraListener != null)
                {
                    return mainCameraListener;
                }
            }

            for (int i = 0; i < listeners.Count; i++)
            {
                AudioListener listener = listeners[i];
                if (listener != null && listener.gameObject.scene == activeScene)
                {
                    return listener;
                }
            }

            return listeners[0];
        }
    }
}
