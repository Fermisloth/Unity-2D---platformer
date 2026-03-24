using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Time until jump force stops being applied
    float jumpEndTime;

    // Movement + jump settings (editable in inspector)
    [SerializeField] float _maxHorizontalSpeed = 5f;
    [SerializeField] float _jumpvelocity = 5f;
    [SerializeField] float _jumpduraion = 0.5f;
    [SerializeField] Sprite _jumpSprite;
    [SerializeField] float _footOffset = 0.5f;
    [SerializeField] float _groundAcceleration = 10f;
    [SerializeField] float _snowAcceleration = 1f;

    // Ground state
    public bool IsGround;
    public bool IsOnSnow;

    // Cached components
    SpriteRenderer _spriteRenderer;
    float _horizontal;

    private Rigidbody2D _rb;
    private Animator _animator;
    private AudioSource _audioSource;

    // Remaining jumps (for double jump etc.)
    int _jumpRemaining;

    private void Awake()
    {
        // Cache components for performance
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    void OnDrawGizmos()
    {
        // Draw debug lines for ground detection rays
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Gizmos.color = Color.red;

        // Center ray
        Vector2 origin = new Vector2(
            transform.position.x,
            transform.position.y - spriteRenderer.bounds.extents.y
        );
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);

        // Left foot ray
        origin = new Vector2(
            transform.position.x - _footOffset,
            transform.position.y - spriteRenderer.bounds.extents.y
        );
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);

        // Right foot ray
        origin = new Vector2(
            transform.position.x + _footOffset,
            transform.position.y - spriteRenderer.bounds.extents.y
        );
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);
    }

    void Update()
    {
        // Update ground detection
        _updateGrounding();

        // Get horizontal input (-1 to 1)
        var horizontalInput = Input.GetAxis("Horizontal");

        // Keep current vertical velocity (gravity or jump)
        float vertical = _rb.linearVelocity.y;

        // Jump start (button pressed)
        if (Input.GetButtonDown("Fire1") && _jumpRemaining > 0)
        {
            // Set how long jump can be held
            jumpEndTime = Time.time + _jumpduraion;

            // Reduce available jumps
            _jumpRemaining--;

            // Slight pitch variation for second jump
            _audioSource.pitch = (_jumpRemaining) > 0 ? 1 : 1.2f;

            // Play jump sound
            _audioSource.Play();
        }

        // Continue jump while button is held
        if (Input.GetButton("Fire1") && jumpEndTime > Time.time)
        {
            vertical = _jumpvelocity;
        }

        // Target horizontal speed based on input
        var desiredHorizontal = horizontalInput * _maxHorizontalSpeed;
        var acceleration = IsOnSnow ? _snowAcceleration : _groundAcceleration;

        // Smoothly move toward target speed (FIXED: assignment, not multiplication)
        _horizontal = Mathf.Lerp(
            _horizontal,
            desiredHorizontal,
            Time.deltaTime * acceleration
        );

        // Apply velocity to Rigidbody
        _rb.linearVelocity = new Vector2(_horizontal, vertical);

        // Update animations and sprite direction
        UpdateSprite();
    }

    private void _updateGrounding()
    {
        IsGround = false;

        // Center ground check
        Vector2 origin = new Vector2(
            transform.position.x,
            transform.position.y - _spriteRenderer.bounds.extents.y
        );

        var hit = Physics2D.Raycast(origin, Vector2.down, 0.1f);
        if (hit.collider)
        {
            IsGround = true;
            IsOnSnow = hit.collider.CompareTag("Snow");
        }
           
        // Right ground check
        origin = new Vector2(
            transform.position.x + _footOffset,
            transform.position.y - _spriteRenderer.bounds.extents.y
        );

        hit = Physics2D.Raycast(origin, Vector2.down, 0.1f);
        if (hit.collider)
        {
            IsGround = true;
            IsOnSnow = hit.collider.CompareTag("Snow");
        }
        // Left ground check
        origin = new Vector2(
            transform.position.x - _footOffset,
            transform.position.y - _spriteRenderer.bounds.extents.y
        );

        hit = Physics2D.Raycast(origin, Vector2.down, 0.1f);
        if (hit.collider)
        {
            IsGround = true;
            IsOnSnow = hit.collider.CompareTag("Snow");
        }
        // Reset jumps when grounded and not moving upward
        if (IsGround && _rb.linearVelocity.y <= 0)
        {
            _jumpRemaining = 2;
        }
    }

    private void UpdateSprite()
    {
        // Update animator parameters
        _animator.SetBool("IsGrounded", IsGround);
        _animator.SetFloat("Horizontal_velocity", Mathf.Abs(_horizontal));

        // Flip sprite based on direction
        if (_horizontal > 0)
            _spriteRenderer.flipX = false;
        else if (_horizontal < 0)
            _spriteRenderer.flipX = true;
    }
}