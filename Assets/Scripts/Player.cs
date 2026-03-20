using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float jumpEndTime;

    [SerializeField] float _horizontalvelocity = 3f;
    [SerializeField] float _jumpvelocity = 5f;
    [SerializeField] float _jumpduraion = 0.5f;
    [SerializeField] Sprite _jumpSprite;
    [SerializeField] float _footOffset = 0.5f;

    public bool IsGround;

    SpriteRenderer _spriteRenderer;
    private float _horizontal;

    private Rigidbody2D _rb;
    private Animator _animator;
    private AudioSource _audioSource;
    int _jumpRemaining;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    void OnDrawGizmos()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Gizmos.color = Color.red;

        Vector2 origin = new Vector2(transform.position.x, transform.position.y - spriteRenderer.bounds.extents.y);
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);

        // Draw Left Foot
        origin = new Vector2(transform.position.x - _footOffset, transform.position.y - spriteRenderer.bounds.extents.y);
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);

        // Draw Right Foot
        origin = new Vector2(transform.position.x + _footOffset, transform.position.y - spriteRenderer.bounds.extents.y);
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);
    }

    void Update()
    {
        _updateGrounding();

        // Input
        _horizontal = Input.GetAxis("Horizontal");

        float vertical = _rb.linearVelocity.y;

        // Jump logic (FIXED: added braces)
        if (Input.GetButtonDown("Fire1") && _jumpRemaining > 0)
        {
            jumpEndTime = Time.time + _jumpduraion;
            _jumpRemaining--;

            _audioSource.pitch = (_jumpRemaining) > 0 ? 1 : 1.2f;

            _audioSource.Play();
            
        }

        if (Input.GetButton("Fire1") && jumpEndTime > Time.time)
            vertical = _jumpvelocity;

        // Apply horizontal movement with cap
        float horizontalVelocity = _horizontal * _horizontalvelocity;

        _rb.linearVelocity = new Vector2(horizontalVelocity, vertical);

        UpdateSprite(horizontalVelocity);
    }

    private void _updateGrounding()
    {
        IsGround = false;

        // Ground center check
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - _spriteRenderer.bounds.extents.y);
        var hit = Physics2D.Raycast(origin, Vector2.down, 0.1f);
        if (hit.collider)
            IsGround = true;

        // Ground right check
        origin = new Vector2(transform.position.x + _footOffset, transform.position.y - _spriteRenderer.bounds.extents.y);
        hit = Physics2D.Raycast(origin, Vector2.down, 0.1f);
        if (hit.collider)
            IsGround = true;

        // Ground left check
        origin = new Vector2(transform.position.x - _footOffset, transform.position.y - _spriteRenderer.bounds.extents.y);
        hit = Physics2D.Raycast(origin, Vector2.down, 0.1f);
        if (hit.collider)
            IsGround = true;

        if (IsGround && _rb.linearVelocity.y <= 0)
            _jumpRemaining = 2;
    }

    private void UpdateSprite(float horizontalVelocity)
    {
        _animator.SetBool("IsGrounded", IsGround);

        _animator.SetFloat("Horizontal_velocity", Mathf.Abs(horizontalVelocity));

        if (horizontalVelocity > 0)
            _spriteRenderer.flipX = false;
        else if (horizontalVelocity < 0)
            _spriteRenderer.flipX = true;
    }
}