using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpiritBond.Core
{
    public static class GameplayTriggerGuard
    {
        private const float DefaultLockDurationSeconds = 5f;

        private static float unlockAtUnscaledTime;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public static bool IsBlocked => Time.unscaledTime < unlockAtUnscaledTime;

        public static float RemainingBlockTime => Mathf.Max(0f, unlockAtUnscaledTime - Time.unscaledTime);

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (string.Equals(scene.name, "Battle", System.StringComparison.Ordinal))
            {
                unlockAtUnscaledTime = Time.unscaledTime;
                return;
            }

            unlockAtUnscaledTime = Time.unscaledTime + DefaultLockDurationSeconds;
            Debug.Log($"[GameplayTriggerGuard] Trigger lock enabled for {DefaultLockDurationSeconds:F1}s on scene {scene.name}.");
        }
    }
}
