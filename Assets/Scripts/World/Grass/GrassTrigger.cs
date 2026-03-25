using SpiritBond.Core;
using UnityEngine;

namespace SpiritBond.World.Grass
{
    [RequireComponent(typeof(Collider2D))]
    public class GrassTrigger : MonoBehaviour
    {
        [SerializeField] private GrassSpawner grassSpawner; // Assign in Inspector

        private void Awake()
        {
            if (grassSpawner == null)
            {
                grassSpawner = GetComponent<GrassSpawner>();
            }
        }

        // Require Collider2D isTrigger
        // Require Player tag
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }

            Debug.Log($"[GrassTrigger] Player entered grass trigger: {gameObject.name}");
            if (!GameplayTriggerGuard.IsBlocked)
            {
                grassSpawner?.TrySpawnEncounter();
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.CompareTag("Player") || GameplayTriggerGuard.IsBlocked)
            {
                return;
            }

            grassSpawner?.TrySpawnEncounter();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }

            grassSpawner?.HandlePlayerExitGrass();
        }
    }
}
