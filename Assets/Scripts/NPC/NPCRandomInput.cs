using UnityEngine;

namespace SpiritBond.NPC
{
    [DisallowMultipleComponent]
    public sealed class NPCRandomInput : MonoBehaviour, IMovementInput
    {
        [SerializeField, Min(0.05f)] private float minDirectionDuration = 1.5f;
        [SerializeField, Min(0.05f)] private float maxDirectionDuration = 3f;
        [SerializeField] private bool includeIdle = true;

        private static readonly Vector2[] DirectionsWithIdle =
        {
            Vector2.zero,
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right
        };

        private static readonly Vector2[] DirectionsWithoutIdle =
        {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right
        };

        private Vector2 currentDirection = Vector2.zero;
        private float directionTimer;

        private void OnEnable()
        {
            PickDirectionAndScheduleNextChange();
        }

        public Vector2 GetMoveInput()
        {
            directionTimer -= Time.deltaTime;
            if (directionTimer <= 0f)
            {
                PickDirectionAndScheduleNextChange();
            }

            return currentDirection;
        }

        private void PickDirectionAndScheduleNextChange()
        {
            Vector2[] pool = includeIdle ? DirectionsWithIdle : DirectionsWithoutIdle;
            currentDirection = pool[Random.Range(0, pool.Length)];
            ScheduleNextChange();
        }

        private void ScheduleNextChange()
        {
            float min = Mathf.Min(minDirectionDuration, maxDirectionDuration);
            float max = Mathf.Max(minDirectionDuration, maxDirectionDuration);
            directionTimer = Random.Range(min, max);
        }

        private void OnValidate()
        {
            minDirectionDuration = Mathf.Max(0.05f, minDirectionDuration);
            maxDirectionDuration = Mathf.Max(0.05f, maxDirectionDuration);
        }
    }
}
