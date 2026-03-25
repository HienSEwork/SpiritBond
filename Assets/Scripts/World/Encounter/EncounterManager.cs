using SpiritBond.Core;
using SpiritBond.Pet;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpiritBond.World.Encounter
{
    public class EncounterManager : SingletonBehaviour<EncounterManager>
    {
        [SerializeField] private string battleSceneName = "Battle"; // Assign in Inspector

        public PetData PendingEnemyPetData { get; private set; }
        public string PendingCheckpointSceneName { get; private set; }
        public Vector3 PendingCheckpointPlayerPosition { get; private set; }
        public bool HasPendingCheckpoint { get; private set; }

        protected override bool DontDestroyOnLoadEnabled => true;

        public void StartEncounter(PetData enemyPetData)
        {
            if (enemyPetData == null)
            {
                Debug.LogWarning("[EncounterManager] StartEncounter failed because enemyPetData is null.");
                return;
            }

            PendingEnemyPetData = enemyPetData;

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

            Debug.Log($"[EncounterManager] Starting encounter with {enemyPetData.petName}. Loading scene: {battleSceneName}");
            SceneManager.LoadScene(battleSceneName);
        }

        public PetData ConsumePendingEnemyPetData()
        {
            PetData pendingPetData = PendingEnemyPetData;
            PendingEnemyPetData = null;
            return pendingPetData;
        }

        public void ClearPendingEncounter()
        {
            Debug.Log("[EncounterManager] Clearing pending encounter data.");
            PendingEnemyPetData = null;
            PendingCheckpointSceneName = string.Empty;
            PendingCheckpointPlayerPosition = Vector3.zero;
            HasPendingCheckpoint = false;
        }
    }
}
