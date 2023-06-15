using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    public event EventHandler OnJump;

    [SerializeField] private float _maxMoveSpeed = 10.0f;
    [SerializeField] private float _maxMoveSpeedInAir = 5.0f;
    [SerializeField] private float _accelerationAmount = 7.0f;
    [SerializeField] private float _deccelerationAmount = 7.0f;

    [SerializeField] private Transform _overlapBoxTransform;
    [SerializeField] private LayerMask _groundLayer;

    [SerializeField] private float _jumpForce = 450.0f;
    [SerializeField] private float _jumpCoyoteTime = 0.5f;
    [SerializeField] private float _jumpInputBufferTime = 0.25f;
    [SerializeField] private float _jumpCutMultiplier = 0.5f;
    [SerializeField] private float _fallGravityMultiplier = 2.0f;

    private bool _isJumping;
    private bool _isFalling;
    private float _lastJumpTime;
    private float _lastGroundedTime;
    
    private Rigidbody2D _rigidbody2D;
    private float _moveInput;
    private bool _wasGrounded;
    private float _gravityScale;

    public bool IsLookingLeft { get; private set; }

    public float MoveSpeed => Mathf.Abs(_rigidbody2D.velocity.x);
    public bool IsFalling => _isFalling;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _gravityScale = _rigidbody2D.gravityScale;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameRunning)
        {
            return;
        }

        if (_lastGroundedTime > 0.0f)
        {
            _lastGroundedTime -= Time.deltaTime;
        }

        if (_lastJumpTime > 0.0f)
        {
            _lastJumpTime -= Time.deltaTime;
        }

        _moveInput = Input.GetAxis("Horizontal");

        if (Mathf.Abs(_moveInput) > 0.0f)
        {
            IsLookingLeft = _moveInput < 0.0f;
        }

        bool isGrounded = Physics2D.OverlapBox(_overlapBoxTransform.position, _overlapBoxTransform.localScale, 0, _groundLayer);
        if (isGrounded)
        {
            if (!_wasGrounded)
            {
                _isJumping = false;
            }

            if (!_isJumping)
            {
                _lastGroundedTime = _jumpCoyoteTime;
            }
        }
        _wasGrounded = isGrounded;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _lastJumpTime = _jumpInputBufferTime;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (_rigidbody2D.velocity.y > 0.0f && _isJumping)
            {
                _rigidbody2D.AddForce(Vector2.down * _rigidbody2D.velocity.y * _jumpCutMultiplier, ForceMode2D.Impulse);
            }
        }

        bool canJump = _lastGroundedTime > 0.0f && _lastJumpTime > 0.0f && !_isJumping;
        if (canJump)
        {
            _rigidbody2D.AddForce(Vector2.up * _jumpForce);
            _isJumping = true;
            _wasGrounded = true;
            _lastGroundedTime = 0.0f;
            _lastJumpTime = 0.0f;
            OnJump.Invoke(this, EventArgs.Empty);
        }

        _isFalling = _rigidbody2D.velocity.y < 0.0f;
        if (_isFalling)
        {
            _rigidbody2D.gravityScale = _gravityScale * _fallGravityMultiplier;
        }
        else
        {
            _rigidbody2D.gravityScale = _gravityScale;
        }
    }

    private void FixedUpdate()
    {
        float maxMoveSpeed = _maxMoveSpeed;

        if (_isJumping)
        {
            maxMoveSpeed = _maxMoveSpeedInAir;
        }
        float targetSpeed = _moveInput * maxMoveSpeed;
        float speedDifferance = targetSpeed - _rigidbody2D.velocity.x;
        
        float accelration = 0.0f;

        bool isTryingToMoveInVelocityDirection = Mathf.Sign(_moveInput) == Mathf.Sign(_rigidbody2D.velocity.x);
        if (isTryingToMoveInVelocityDirection)
        {
            accelration = _accelerationAmount;
        }
        else
        {
            accelration = _deccelerationAmount;
        }

        float moveAmount = accelration * speedDifferance * 2.0f;
        _rigidbody2D.AddForce(moveAmount * Vector2.right);

        bool isStopping = Mathf.Abs(_moveInput) < 0.01f;
        if (isStopping)
        {
            float frictionAmount = 1.0f;
            Vector2 MoveDirection = Vector2.right * Mathf.Sign(_rigidbody2D.velocity.x);
            float stopAmount = Mathf.Min(frictionAmount, Mathf.Abs(_rigidbody2D.velocity.x));
            _rigidbody2D.AddForce(stopAmount * -MoveDirection, ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_overlapBoxTransform.position, _overlapBoxTransform.localScale);
    }
}
