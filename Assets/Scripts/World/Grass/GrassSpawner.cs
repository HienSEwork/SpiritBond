using SpiritBond.Pet;
using SpiritBond.World.Encounter;
using UnityEngine;

namespace SpiritBond.World.Grass
{
    public class GrassSpawner : MonoBehaviour
    {
        [SerializeField] private EncounterConfig encounterConfig; // Assign in Inspector
        [SerializeField, Range(0f, 1f)] private float spawnRate = 0.2f;

        public void TrySpawnEncounter()
        {
            if (encounterConfig == null)
            {
                Debug.LogWarning($"[GrassSpawner] Missing EncounterConfig on {gameObject.name}");
                return;
            }

            float encounterRate = Mathf.Clamp01(spawnRate > 0f ? spawnRate : encounterConfig.SpawnRate);
            float roll = Random.value;
            Debug.Log($"[GrassSpawner] Encounter check on {gameObject.name} | roll={roll:F2} rate={encounterRate:F2}");

            if (roll > encounterRate)
            {
                Debug.Log("[GrassSpawner] No encounter triggered.");
                return;
            }

            PetData randomPet = encounterConfig.GetRandomPet();
            if (randomPet == null)
            {
                Debug.LogWarning("[GrassSpawner] Encounter triggered but no PetData available.");
                return;
            }

            if (EncounterManager.Instance == null)
            {
                Debug.LogWarning("[GrassSpawner] EncounterManager instance not found.");
                return;
            }

            Debug.Log($"[GrassSpawner] Encounter triggered with pet: {randomPet.petName}");
            EncounterManager.Instance.StartEncounter(randomPet);
        }
    }
}
