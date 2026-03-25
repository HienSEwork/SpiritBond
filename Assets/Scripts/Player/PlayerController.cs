using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;

    private Rigidbody2D rb;
    private Animator anim;

    private Vector2 move;
    private Vector2 lastMove;
    private int currentAnimStateHash;
    private bool animatorAvailable = true;
    private bool missingAnimatorWarningLogged;

    private static readonly int MoveXHash = Animator.StringToHash("moveX");
    private static readonly int MoveYHash = Animator.StringToHash("moveY");
    private static readonly int LastXHash = Animator.StringToHash("lastX");
    private static readonly int LastYHash = Animator.StringToHash("lastY");
    private static readonly int WalkUpHash = Animator.StringToHash("npc_walk_up");
    private static readonly int WalkDownHash = Animator.StringToHash("npc_walk_down");
    private static readonly int WalkLeftHash = Animator.StringToHash("npc_walk_left");
    private static readonly int WalkRightHash = Animator.StringToHash("npc_walk_right");
    private static readonly int IdleUpHash = Animator.StringToHash("npc_idle_up");
    private static readonly int IdleDownHash = Animator.StringToHash("npc_idle_down");
    private static readonly int IdleLeftHash = Animator.StringToHash("npc_idle_left");
    private static readonly int IdleRightHash = Animator.StringToHash("npc_idle_right");

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        lastMove = Vector2.down;
        currentAnimStateHash = IdleDownHash;

        if (anim != null)
        {
            animatorAvailable = anim.runtimeAnimatorController != null;
            if (animatorAvailable)
            {
                anim.Play(currentAnimStateHash, 0, 0f);
            }
        }
    }

    private void Update()
    {
        HandleInput();
        HandleAnimation();
    }

    private void FixedUpdate()
    {
        rb.velocity = move * speed;
    }

    private void HandleInput()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        Vector2 rawInput = new Vector2(inputX, inputY);

        if (rawInput.x != 0f)
        {
            move = new Vector2(rawInput.x, 0f);
        }
        else
        {
            move = new Vector2(0f, rawInput.y);
        }

        move = move.normalized;

        if (Mathf.Abs(move.x) < 0.001f) move.x = 0f;
        if (Mathf.Abs(move.y) < 0.001f) move.y = 0f;

        if (move != Vector2.zero)
        {
            lastMove = move;
        }
    }

    private void HandleAnimation()
    {
        if (!animatorAvailable)
        {
            if (!missingAnimatorWarningLogged)
            {
                Debug.LogWarning($"[PlayerController] Missing AnimatorController on {gameObject.name}. Animation updates skipped.");
                missingAnimatorWarningLogged = true;
            }

            return;
        }

        anim.SetFloat(MoveXHash, move.x);
        anim.SetFloat(MoveYHash, move.y);
        anim.SetFloat(LastXHash, lastMove.x);
        anim.SetFloat(LastYHash, lastMove.y);

        int targetStateHash = GetAnimationStateHash();
        if (targetStateHash != currentAnimStateHash)
        {
            anim.Play(targetStateHash, 0, 0f);
            currentAnimStateHash = targetStateHash;
        }
    }

    private int GetAnimationStateHash()
    {
        Vector2 direction = move != Vector2.zero ? move : lastMove;
        bool isMoving = move != Vector2.zero;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x < 0f)
            {
                return isMoving ? WalkLeftHash : IdleLeftHash;
            }

            return isMoving ? WalkRightHash : IdleRightHash;
        }

        if (direction.y > 0f)
        {
            return isMoving ? WalkUpHash : IdleUpHash;
        }

        return isMoving ? WalkDownHash : IdleDownHash;
    }
}
