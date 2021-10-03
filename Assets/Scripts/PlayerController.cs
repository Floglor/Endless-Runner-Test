using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask raycastLayerMask;
    [SerializeField] private float jumpVelocity;
    [SerializeField] private float moveSpeed;
    private CircleCollider2D _collider2D;

    private IInputController _inputController;
    private bool _isReversedGravity;
    private Rigidbody2D _rigidbody2D;

    private bool _alreadyChangedGravity;

    public bool IsMoving { get; set; }
    public bool IsControlsBlocked { get; set; }

    private void Start()
    {
        _inputController = GetComponent<IInputController>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<CircleCollider2D>();
    }


    private void Update()
    {
        HandleJump();
        Move();
    }

    private void Move()
    {
        if (IsMoving)
            _rigidbody2D.velocity = new Vector2(moveSpeed, _rigidbody2D.velocity.y);
    }

    private void HandleJump()
    {
        if (IsControlsBlocked) return;
        if (!_inputController.GetInput()) return;
        if (IsGrounded())
            Jump();
        else
            ReverseGravity();
    }

    private void Jump(float jumpVelocityDivider = 1f)
    {
        _alreadyChangedGravity = false;
        _rigidbody2D.velocity = (_isReversedGravity ? Vector2.down : Vector2.up) * (jumpVelocity * jumpVelocityDivider);
    }

    private void ReverseGravity()
    {
        Jump(0.3f);
        if (_alreadyChangedGravity) return;
        _alreadyChangedGravity = true;
        float gravityScale = _rigidbody2D.gravityScale;

        gravityScale *= -1;
        _rigidbody2D.gravityScale = gravityScale;

        _isReversedGravity = !(gravityScale > 0);
    }

    private bool IsGrounded()
    {
        Bounds bounds = _collider2D.bounds;
        RaycastHit2D raycastHit2D = Physics2D.CircleCast(bounds.center, bounds.size.y,
            Vector2.down * .1f, 0.1f, raycastLayerMask);

        return raycastHit2D.collider != null;
    }
}