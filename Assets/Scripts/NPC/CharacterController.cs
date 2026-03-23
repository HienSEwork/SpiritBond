using UnityEngine;

namespace SpiritBond.NPC
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    [RequireComponent(typeof(Animator))]
    public sealed class CharacterController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField, Min(0f)] private float speed = 5f;

        private Rigidbody2D rb;
        private Animator anim;
        private IMovementInput movementInput;

        private Vector2 move;
        private Vector2 lastMove = Vector2.down;

        private static readonly int MoveXHash = Animator.StringToHash("moveX");
        private static readonly int MoveYHash = Animator.StringToHash("moveY");
        private static readonly int LastXHash = Animator.StringToHash("lastX");
        private static readonly int LastYHash = Animator.StringToHash("lastY");

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();

            movementInput = ResolveMovementInput();
            if (movementInput == null)
            {
                enabled = false;
                rb.velocity = Vector2.zero;
                return;
            }
        }

        private void Update()
        {
            ReadMovement();
            UpdateAnimator();
        }

        private void FixedUpdate()
        {
            rb.velocity = move * speed;
        }

        private void ReadMovement()
        {
            Vector2 rawInput = movementInput.GetMoveInput();

            // No diagonal movement: prioritize X axis.
            if (Mathf.Abs(rawInput.x) > 0f)
            {
                rawInput.y = 0f;
            }

            move = rawInput.normalized;

            if (Mathf.Abs(move.x) < 0.1f) move.x = 0f;
            if (Mathf.Abs(move.y) < 0.1f) move.y = 0f;

            if (move != Vector2.zero)
            {
                lastMove = move;
            }
        }

        private void UpdateAnimator()
        {
            anim.SetFloat(MoveXHash, move.x);
            anim.SetFloat(MoveYHash, move.y);
            anim.SetFloat(LastXHash, lastMove.x);
            anim.SetFloat(LastYHash, lastMove.y);
        }

        private IMovementInput ResolveMovementInput()
        {
            MonoBehaviour[] components = GetComponents<MonoBehaviour>();
            IMovementInput found = null;
            int count = 0;

            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] is IMovementInput input)
                {
                    count++;
                    if (found == null)
                    {
                        found = input;
                    }
                }
            }

            if (count == 1)
            {
                return found;
            }

            if (count == 0)
            {
                Debug.LogError($"{name}: Missing IMovementInput implementation on character.", this);
            }
            else
            {
                Debug.LogError($"{name}: Multiple IMovementInput implementations found ({count}). Keep exactly one.", this);
            }

            return null;
        }
    }
}
