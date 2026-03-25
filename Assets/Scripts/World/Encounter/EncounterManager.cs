using SpiritBond.Core;
using SpiritBond.Pet;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpiritBond.World.Encounter
{
    public class EncounterManager : SingletonBehaviour<EncounterManager>
    {
        private static bool creatingRuntimeInstance;

        [SerializeField] private string battleSceneName = "Battle"; // Assign in Inspector

        public PetData PendingEnemyPetData { get; private set; }
        public int PendingEnemyLevel { get; private set; } = PetProgression.MinLevel;
        public string PendingCheckpointSceneName { get; private set; }
        public Vector3 PendingCheckpointPlayerPosition { get; private set; }
        public bool HasPendingCheckpoint { get; private set; }

        protected override bool DontDestroyOnLoadEnabled => true;

        protected override void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }

            if (!creatingRuntimeInstance && transform.childCount > 0)
            {
                creatingRuntimeInstance = true;

                GameObject runtimeHost = new GameObject("EncounterManager");
                EncounterManager runtimeManager = runtimeHost.AddComponent<EncounterManager>();
                runtimeManager.battleSceneName = battleSceneName;

                creatingRuntimeInstance = false;
                Destroy(this);
                return;
            }

            base.Awake();
        }

        public void StartEncounter(PetData enemyPetData)
        {
            StartEncounter(enemyPetData, PetProgression.MinLevel);
        }

        public void StartEncounter(PetData enemyPetData, int enemyLevel)
        {
            if (enemyPetData == null)
            {
                Debug.LogWarning("[EncounterManager] StartEncounter failed because enemyPetData is null.");
                return;
            }

            PendingEnemyPetData = enemyPetData;
            PendingEnemyLevel = Mathf.Max(PetProgression.MinLevel, enemyLevel);

            if (SaveGameService.TryGetCurrentWorldState(out string sceneName, out Vector3 playerPosition))
            {
                PendingCheckpointSceneName = sceneName;
                PendingCheckpointPlayerPosition = playerPosition;
                HasPendingCheckpoint = true;
                SaveGameService.SetBattleCheckpoint(sceneName, playerPosition);
            }
            else
            {
                HasPendingCheckpoint = false;
                PendingCheckpointSceneName = string.Empty;
                PendingCheckpointPlayerPosition = Vector3.zero;
            }

            Debug.Log($"[EncounterManager] Starting encounter with {enemyPetData.petName} at level {PendingEnemyLevel}. Loading scene: {battleSceneName}");
            SceneManager.LoadScene(battleSceneName);
        }

        public PetData ConsumePendingEnemyPetData()
        {
            PetData pendingPetData = PendingEnemyPetData;
            PendingEnemyPetData = null;
            return pendingPetData;
        }

        public int ConsumePendingEnemyLevel()
        {
            int pendingLevel = PendingEnemyLevel;
            PendingEnemyLevel = PetProgression.MinLevel;
            return Mathf.Max(PetProgression.MinLevel, pendingLevel);
        }

        public void ClearPendingEncounter()
        {
            Debug.Log("[EncounterManager] Clearing pending encounter data.");
            PendingEnemyPetData = null;
            PendingEnemyLevel = PetProgression.MinLevel;
            PendingCheckpointSceneName = string.Empty;
            PendingCheckpointPlayerPosition = Vector3.zero;
            HasPendingCheckpoint = false;
        }
    }
}
