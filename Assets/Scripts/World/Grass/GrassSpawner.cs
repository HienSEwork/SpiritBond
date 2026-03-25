using SpiritBond.Pet;
using SpiritBond.World.Encounter;
using UnityEngine;

namespace SpiritBond.World.Grass
{
    [RequireComponent(typeof(Collider2D))]
    public class GrassSpawner : MonoBehaviour
    {
        [SerializeField] private EncounterConfig encounterConfig; // Assign in Inspector
        [SerializeField, Range(0f, 1f)] private float spawnRate = 0.2f;
        [SerializeField] private GameObject roamingPetPrefab;
        [SerializeField] private float respawnCooldown = 2f;
        [SerializeField] private float roamingMoveSpeed = 1.2f;
        [SerializeField] private float roamingIdleDuration = 0.5f;
        [SerializeField] private float roamingArrivalDistance = 0.08f;
        [SerializeField] private bool despawnPetWhenPlayerLeaves = true;

        private Collider2D grassAreaCollider;
        private GrassEncounterPet activeRoamingPet;
        private float nextSpawnAllowedTime;

        private void Awake()
        {
            grassAreaCollider = GetComponent<Collider2D>();
        }

        public void TrySpawnEncounter()
        {
            if (activeRoamingPet != null || Time.time < nextSpawnAllowedTime)
            {
                return;
            }

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

            SpawnRoamingPet(randomPet);
        }

        public void DespawnRoamingPet()
        {
            if (activeRoamingPet == null)
            {
                return;
            }

            Destroy(activeRoamingPet.gameObject);
            activeRoamingPet = null;
            nextSpawnAllowedTime = Time.time + respawnCooldown;
        }

        public void HandlePlayerExitGrass()
        {
            if (despawnPetWhenPlayerLeaves)
            {
                DespawnRoamingPet();
            }
        }

        public void StartEncounterFromRoamingPet(GrassEncounterPet roamingPet)
        {
            if (roamingPet == null || roamingPet != activeRoamingPet)
            {
                return;
            }

            PetData enemyPetData = roamingPet.PetData;
            activeRoamingPet = null;
            nextSpawnAllowedTime = Time.time + respawnCooldown;

            if (enemyPetData == null)
            {
                Debug.LogWarning("[GrassSpawner] Roaming pet did not have PetData.");
                if (roamingPet != null)
                {
                    Destroy(roamingPet.gameObject);
                }

                return;
            }

            Debug.Log($"[GrassSpawner] Player touched roaming pet: {enemyPetData.petName}");
            Destroy(roamingPet.gameObject);
            EncounterManager.Instance.StartEncounter(enemyPetData);
        }

        internal Vector3 GetRandomSpawnPosition()
        {
            if (grassAreaCollider == null)
            {
                return transform.position;
            }

            Bounds bounds = grassAreaCollider.bounds;
            return new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                0f
            );
        }

        private void SpawnRoamingPet(PetData petData)
        {
            GameObject petObject = roamingPetPrefab != null
                ? Instantiate(roamingPetPrefab, GetRandomSpawnPosition(), Quaternion.identity, transform)
                : CreateFallbackRoamingPetObject();

            if (petObject == null)
            {
                Debug.LogWarning("[GrassSpawner] Failed to create roaming pet object.");
                return;
            }

            petObject.transform.position = GetRandomSpawnPosition();

            GrassEncounterPet roamingPet = petObject.GetComponent<GrassEncounterPet>();
            if (roamingPet == null)
            {
                roamingPet = petObject.AddComponent<GrassEncounterPet>();
            }

            roamingPet.Initialize(this, petData, roamingMoveSpeed, roamingIdleDuration, roamingArrivalDistance);
            activeRoamingPet = roamingPet;
            nextSpawnAllowedTime = Time.time + respawnCooldown;

            Debug.Log($"[GrassSpawner] Spawned roaming pet: {petData.petName}");
        }

        private GameObject CreateFallbackRoamingPetObject()
        {
            GameObject petObject = new GameObject("GrassEncounterPet");
            petObject.transform.SetParent(transform);

            SpriteRenderer spriteRenderer = petObject.AddComponent<SpriteRenderer>();
            CircleCollider2D triggerCollider = petObject.AddComponent<CircleCollider2D>();
            Rigidbody2D rigidbody2D = petObject.AddComponent<Rigidbody2D>();

            triggerCollider.isTrigger = true;
            triggerCollider.radius = 0.2f;

            rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            rigidbody2D.gravityScale = 0f;
            rigidbody2D.freezeRotation = true;

            spriteRenderer.sortingOrder = 10;
            return petObject;
        }
    }
}
