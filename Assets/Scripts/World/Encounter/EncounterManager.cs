using SpiritBond.Core;
using SpiritBond.Pet;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpiritBond.World.Encounter
{
    public class EncounterManager : SingletonBehaviour<EncounterManager>
    {
        [SerializeField] private string battleSceneName = "BattleScene"; // Assign in Inspector

        public PetData PendingEnemyPetData { get; private set; }

        protected override bool DontDestroyOnLoadEnabled => true;

        public void StartEncounter(PetData enemyPetData)
        {
            if (enemyPetData == null)
            {
                Debug.LogWarning("[EncounterManager] StartEncounter failed because enemyPetData is null.");
                return;
            }

            PendingEnemyPetData = enemyPetData;
            Debug.Log($"[EncounterManager] Starting encounter with {enemyPetData.petName}. Loading scene: {battleSceneName}");
            SceneManager.LoadScene(battleSceneName);
        }

        public void ClearPendingEncounter()
        {
            Debug.Log("[EncounterManager] Clearing pending encounter data.");
            PendingEnemyPetData = null;
        }
    }
}
