using System;
using System.IO;
using SpiritBond.Pet;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpiritBond.Core
{
    public static class SaveGameService
    {
        private const string SaveFileName = "autosave.json";
        private const string DefaultWorldSceneName = "Map Forest";

        private static bool initialized;

        public static PlayerProgressState CurrentState { get; private set; }

        private static GameSaveData currentSaveData;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RuntimeInitialize()
        {
            if (initialized)
            {
                return;
            }

            initialized = true;
            SceneManager.sceneLoaded += OnSceneLoaded;
            Application.quitting += OnApplicationQuitting;
            LoadOrCreate();
        }

        public static PlayerProgressState LoadOrCreate()
        {
            if (CurrentState != null && currentSaveData != null)
            {
                return CurrentState;
            }

            string savePath = GetSavePath();
            if (!File.Exists(savePath))
            {
                CreateInitialSave();
                return CurrentState;
            }

            try
            {
                string json = File.ReadAllText(savePath);
                GameSaveData loadedData = JsonUtility.FromJson<GameSaveData>(json);

                if (loadedData == null)
                {
                    throw new InvalidDataException("Loaded save data is null.");
                }

                currentSaveData = loadedData;
                CurrentState = BuildStateFromSave(loadedData);
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"[SaveGameService] Load failed. Creating new save. Reason: {exception.Message}");
                CreateInitialSave();
            }

            return CurrentState;
        }

        public static void CreateInitialSave()
        {
            PlayerProgressState state = new PlayerProgressState();
            PetInstance firefox = SamplePetContent.CreateFirefox();
            state.AddOwnedPet(firefox);
            state.SetBattleTeamSlot(0, firefox.instanceId);
            state.SetBattleTeamSlot(1, null);
            state.SetBattleTeamSlot(2, null);

            CurrentState = state;
            currentSaveData = new GameSaveData
            {
                currentSceneName = DefaultWorldSceneName,
                hasCurrentPlayerPosition = false,
                currentPlayerPosition = SerializableVector3.Zero,
                battleCheckpoint = new WorldCheckpointSaveData
                {
                    sceneName = DefaultWorldSceneName,
                    hasPlayerPosition = false,
                    playerPosition = SerializableVector3.Zero
                },
                battleTeamInstanceIds = state.GetBattleTeamSnapshot(),
                allOwnedPets = Array.Empty<PetInstanceSaveData>()
            };

            Save();
        }

        public static void Save()
        {
            LoadOrCreate();
            currentSaveData.battleTeamInstanceIds = CurrentState.GetBattleTeamSnapshot();
            currentSaveData.allOwnedPets = BuildOwnedPetSaveData(CurrentState);

            string json = JsonUtility.ToJson(currentSaveData, true);
            File.WriteAllText(GetSavePath(), json);
        }

        public static void SaveCurrentWorldState(string sceneName, Vector3 playerPosition)
        {
            LoadOrCreate();
            currentSaveData.currentSceneName = sceneName;
            currentSaveData.hasCurrentPlayerPosition = true;
            currentSaveData.currentPlayerPosition = SerializableVector3.FromVector3(playerPosition);
            Save();
        }

        public static void SetBattleCheckpoint(string sceneName, Vector3 playerPosition)
        {
            LoadOrCreate();
            currentSaveData.currentSceneName = sceneName;
            currentSaveData.hasCurrentPlayerPosition = true;
            currentSaveData.currentPlayerPosition = SerializableVector3.FromVector3(playerPosition);
            currentSaveData.battleCheckpoint = new WorldCheckpointSaveData
            {
                sceneName = sceneName,
                hasPlayerPosition = true,
                playerPosition = SerializableVector3.FromVector3(playerPosition)
            };
            Save();
        }

        public static void ReturnToLatestCheckpoint()
        {
            LoadOrCreate();

            string returnSceneName = currentSaveData.currentSceneName;
            if (currentSaveData.battleCheckpoint != null && !string.IsNullOrWhiteSpace(currentSaveData.battleCheckpoint.sceneName))
            {
                returnSceneName = currentSaveData.battleCheckpoint.sceneName;

                if (currentSaveData.battleCheckpoint.hasPlayerPosition)
                {
                    currentSaveData.currentSceneName = currentSaveData.battleCheckpoint.sceneName;
                    currentSaveData.hasCurrentPlayerPosition = true;
                    currentSaveData.currentPlayerPosition = currentSaveData.battleCheckpoint.playerPosition;
                }
            }

            Save();
            SceneManager.LoadScene(returnSceneName);
        }

        public static bool TryGetCurrentWorldState(out string sceneName, out Vector3 playerPosition)
        {
            sceneName = string.Empty;
            playerPosition = Vector3.zero;

            Scene currentScene = SceneManager.GetActiveScene();
            if (!currentScene.IsValid() || string.IsNullOrWhiteSpace(currentScene.name))
            {
                return false;
            }

            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject == null)
            {
                return false;
            }

            sceneName = currentScene.name;
            playerPosition = playerObject.transform.position;
            return true;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            LoadOrCreate();

            if (currentSaveData == null || !currentSaveData.hasCurrentPlayerPosition)
            {
                return;
            }

            if (!string.Equals(scene.name, currentSaveData.currentSceneName, StringComparison.Ordinal))
            {
                return;
            }

            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject == null)
            {
                return;
            }

            playerObject.transform.position = currentSaveData.currentPlayerPosition.ToVector3();
        }

        private static void OnApplicationQuitting()
        {
            if (!initialized)
            {
                return;
            }

            if (TryGetCurrentWorldState(out string sceneName, out Vector3 playerPosition))
            {
                currentSaveData.currentSceneName = sceneName;
                currentSaveData.hasCurrentPlayerPosition = true;
                currentSaveData.currentPlayerPosition = SerializableVector3.FromVector3(playerPosition);
            }

            Save();
        }

        private static PlayerProgressState BuildStateFromSave(GameSaveData saveData)
        {
            PlayerProgressState state = new PlayerProgressState();

            if (saveData.allOwnedPets != null)
            {
                for (int i = 0; i < saveData.allOwnedPets.Length; i++)
                {
                    PetInstance petInstance = PetInstance.FromSaveData(saveData.allOwnedPets[i]);
                    if (petInstance != null)
                    {
                        state.AddOwnedPet(petInstance);
                    }
                }
            }

            if (saveData.battleTeamInstanceIds != null)
            {
                for (int i = 0; i < saveData.battleTeamInstanceIds.Length && i < PlayerProgressState.BattleTeamSize; i++)
                {
                    state.SetBattleTeamSlot(i, saveData.battleTeamInstanceIds[i]);
                }
            }

            if (state.AllOwnedPets.Count == 0)
            {
                PetInstance firefox = SamplePetContent.CreateFirefox();
                state.AddOwnedPet(firefox);
                state.SetBattleTeamSlot(0, firefox.instanceId);
            }

            return state;
        }

        private static PetInstanceSaveData[] BuildOwnedPetSaveData(PlayerProgressState state)
        {
            PetInstanceSaveData[] petSaveData = new PetInstanceSaveData[state.AllOwnedPets.Count];

            for (int i = 0; i < state.AllOwnedPets.Count; i++)
            {
                petSaveData[i] = state.AllOwnedPets[i].ToSaveData();
            }

            return petSaveData;
        }

        private static string GetSavePath()
        {
            return Path.Combine(Application.persistentDataPath, SaveFileName);
        }
    }

    [Serializable]
    public class GameSaveData
    {
        public string currentSceneName;
        public bool hasCurrentPlayerPosition;
        public SerializableVector3 currentPlayerPosition;
        public WorldCheckpointSaveData battleCheckpoint;
        public PetInstanceSaveData[] allOwnedPets;
        public string[] battleTeamInstanceIds;
    }

    [Serializable]
    public class WorldCheckpointSaveData
    {
        public string sceneName;
        public bool hasPlayerPosition;
        public SerializableVector3 playerPosition;
    }

    [Serializable]
    public struct SerializableVector3
    {
        public float x;
        public float y;
        public float z;

        public static SerializableVector3 Zero => new SerializableVector3 { x = 0f, y = 0f, z = 0f };

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }

        public static SerializableVector3 FromVector3(Vector3 value)
        {
            return new SerializableVector3
            {
                x = value.x,
                y = value.y,
                z = value.z
            };
        }
    }
}
