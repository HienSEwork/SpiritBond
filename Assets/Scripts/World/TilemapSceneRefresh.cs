using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace SpiritBond.World
{
    public static class TilemapSceneRefresh
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RuntimeInitialize()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (!scene.IsValid() || string.IsNullOrWhiteSpace(scene.name) || scene.name == "Battle")
            {
                return;
            }

            GameObject[] rootObjects = scene.GetRootGameObjects();
            for (int i = 0; i < rootObjects.Length; i++)
            {
                Tilemap[] tilemaps = rootObjects[i].GetComponentsInChildren<Tilemap>(true);
                for (int j = 0; j < tilemaps.Length; j++)
                {
                    Tilemap tilemap = tilemaps[j];
                    if (tilemap == null)
                    {
                        continue;
                    }

                    tilemap.CompressBounds();
                    tilemap.RefreshAllTiles();

                    TilemapRenderer renderer = tilemap.GetComponent<TilemapRenderer>();
                    if (renderer != null)
                    {
                        renderer.enabled = false;
                        renderer.enabled = true;
                    }
                }
            }

            Debug.Log($"[TilemapSceneRefresh] Refreshed tilemaps for scene {scene.name}.");
        }
    }
}
