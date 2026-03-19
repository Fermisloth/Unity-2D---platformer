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

    public bool IsGround;

    private SpriteRenderer _spriteRenderer;
    private Sprite _defaultSprite;
    private float _horizontal;

    private Rigidbody2D _rb;
    private Animator _animator;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultSprite = _spriteRenderer.sprite;

        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void OnDrawGizmos()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - spriteRenderer.bounds.extents.y);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);
    }

    void Update()
    {
        // Ground check
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - _spriteRenderer.bounds.extents.y);
        var hit = Physics2D.Raycast(origin, Vector2.down, 0.1f);

        IsGround = hit.collider != null;

        // Input
        _horizontal = Input.GetAxis("Horizontal");

        float vertical = _rb.linearVelocity.y;

        // Jump logic
        if (Input.GetButtonDown("Fire1") && IsGround)
            jumpEndTime = Time.time + _jumpduraion;

        if (Input.GetButton("Fire1") && jumpEndTime > Time.time)
            vertical = _jumpvelocity;

        // Apply horizontal movement with cap
        float horizontalVelocity = _horizontal * _horizontalvelocity;

        _rb.linearVelocity = new Vector2(horizontalVelocity, vertical);

        UpdateSprite(horizontalVelocity);
    }

    private void UpdateSprite(float horizontalVelocity)
    {
        _animator.SetBool("IsGrounded", IsGround);

        // Use ABS value so left/right doesn't break transitions
        _animator.SetFloat("Horizontal_velocity", Mathf.Abs(horizontalVelocity));

        // Flip sprite
        if (horizontalVelocity > 0)
            _spriteRenderer.flipX = false;
        else if (horizontalVelocity < 0)
            _spriteRenderer.flipX = true;
    }
}