using SpiritBond.Pet;
using UnityEngine;

namespace SpiritBond.World.Grass
{
    [RequireComponent(typeof(Collider2D))]
    public class GrassEncounterPet : MonoBehaviour
    {
        private GrassSpawner owner;
        private SpriteRenderer spriteRenderer;
        private PetData petData;
        private float moveSpeed;
        private float idleDuration;
        private float arrivalDistance;
        private float idleTimer;
        private bool encounterStarted;
        private Vector3 targetPosition;

        public PetData PetData => petData;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Initialize(GrassSpawner grassSpawner, PetData data, float speed, float idleTime, float reachDistance)
        {
            owner = grassSpawner;
            petData = data;
            moveSpeed = Mathf.Max(0.1f, speed);
            idleDuration = Mathf.Max(0f, idleTime);
            arrivalDistance = Mathf.Max(0.01f, reachDistance);
            idleTimer = 0f;
            targetPosition = owner != null ? owner.GetRandomSpawnPosition() : transform.position;

            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }

            if (spriteRenderer != null && petData != null)
            {
                spriteRenderer.sprite = petData.avatar != null ? petData.avatar : petData.battleSprite;
            }

            Debug.Log($"[GrassEncounterPet] Initialized roaming pet {GetPetName()} on {gameObject.name}");
        }

        private void Update()
        {
            if (owner == null || encounterStarted)
            {
                return;
            }

            if (idleTimer > 0f)
            {
                idleTimer -= Time.deltaTime;
                return;
            }

            Vector3 currentPosition = transform.position;
            currentPosition.z = 0f;
            transform.position = currentPosition;

            Vector3 nextPosition = Vector3.MoveTowards(currentPosition, targetPosition, moveSpeed * Time.deltaTime);
            transform.position = nextPosition;

            if (spriteRenderer != null)
            {
                float directionX = targetPosition.x - currentPosition.x;
                if (Mathf.Abs(directionX) > 0.01f)
                {
                    spriteRenderer.flipX = directionX < 0f;
                }
            }

            if (Vector3.Distance(nextPosition, targetPosition) <= arrivalDistance)
            {
                targetPosition = owner.GetRandomSpawnPosition();
                idleTimer = idleDuration;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log($"[GrassEncounterPet] OnTriggerEnter2D by {other.name} | Tag={other.tag}");

            if (encounterStarted || !other.CompareTag("Player"))
            {
                return;
            }

            encounterStarted = true;
            Debug.Log($"[GrassEncounterPet] Player touched roaming pet {GetPetName()}.");
            owner?.StartEncounterFromRoamingPet(this);
        }

        private string GetPetName()
        {
            return petData != null ? petData.petName : "UnknownPet";
        }
    }
}
