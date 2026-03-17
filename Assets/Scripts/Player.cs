using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float jumpEndTime;
    [SerializeField] float _horizontalvelocity = 3;
    [SerializeField] float _jumpvelocity = 5;
    [SerializeField] float _jumpduraion = 0.5f;
    [SerializeField] Sprite _jumpSprite;
    public bool IsGround;
    private SpriteRenderer _spriteRenderer;
    private Sprite _defaultSprite;
    private float _horizontal;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultSprite = _spriteRenderer.sprite;
    }
    void OnDrawGizmos()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - spriteRenderer.bounds.extents.y);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);
    }

    // Update is called once per fram
    void Update()
    {
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - _spriteRenderer.bounds.extents.y);
        var hit = Physics2D.Raycast(origin, Vector2.down, 0.1f);
        if (hit.collider)
            IsGround = true;
        else
            IsGround = false;

         _horizontal = Input.GetAxis("Horizontal");
        Debug.Log(_horizontal);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        var vertiacal = rb.linearVelocity.y;

        if (Input.GetButtonDown("Fire1") && IsGround)

            jumpEndTime = Time.time + _jumpduraion;


        if (Input.GetButton("Fire1") && jumpEndTime > Time.time)

            vertiacal = _jumpvelocity;
        _horizontal *= _horizontalvelocity;
        rb.linearVelocity = new Vector2(_horizontal, vertiacal);
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (IsGround)
           _spriteRenderer.sprite = _defaultSprite;
        else
           _spriteRenderer.sprite = _jumpSprite;
        if (_horizontal > 0)
           _spriteRenderer.flipX = false;
        else if (_horizontal < 0)
           _spriteRenderer.flipX = true; 
    }
}
