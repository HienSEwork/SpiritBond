using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private MonoBehaviour movementInputSource;

    private Rigidbody2D rb;
    private Animator anim;
    private IMovementInput movementInput;

    private Vector2 move;
    private Vector2 lastMove;

    private static readonly int MoveXHash = Animator.StringToHash("moveX");
    private static readonly int MoveYHash = Animator.StringToHash("moveY");
    private static readonly int LastXHash = Animator.StringToHash("lastX");
    private static readonly int LastYHash = Animator.StringToHash("lastY");

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        movementInput = ResolveInputSource();
        lastMove = Vector2.down;
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
        Vector2 rawInput = movementInput != null ? movementInput.GetMoveInput() : Vector2.zero;

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
        anim.SetFloat(MoveXHash, move.x);
        anim.SetFloat(MoveYHash, move.y);
        anim.SetFloat(LastXHash, lastMove.x);
        anim.SetFloat(LastYHash, lastMove.y);
    }

    private IMovementInput ResolveInputSource()
    {
        if (movementInputSource != null)
        {
            IMovementInput explicitSource = movementInputSource as IMovementInput;
            if (explicitSource != null)
                return explicitSource;

            Debug.LogError($"{name}: Assigned movementInputSource does not implement IMovementInput.", this);
        }

        IMovementInput localSource = GetComponent<IMovementInput>();
        if (localSource != null)
            return localSource;

        Debug.LogWarning($"{name}: No IMovementInput found. Falling back to idle.", this);
        return null;
    }
}
