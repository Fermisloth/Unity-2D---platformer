using System;
using UnityEngine;

public class Frog : MonoBehaviour
{
    Rigidbody2D _rb;
    SpriteRenderer _spriteRenderer;
    Sprite _defaultSprite;
    int _jumpsRemaining;

    [SerializeField] float _jumpDelay = 3;
    [SerializeField] Vector2 _jumpForce;
    [SerializeField] Sprite _jumpSprite;
    [SerializeField] int _jumps = 2;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultSprite = _spriteRenderer.sprite;
        _jumpsRemaining = _jumps;

        InvokeRepeating("Jump", _jumpDelay, _jumpDelay);
        
    }
    
    void Jump()
    {
        if (_jumpsRemaining <= 0)
        {
            _jumpForce *= new Vector2(-1, 1);
            _jumpsRemaining = _jumps;
        }
        _jumpsRemaining--;
        _rb.AddForce(_jumpForce);
        
        _spriteRenderer.flipX = _jumpForce.x > 0 ;
        _spriteRenderer.sprite = _jumpSprite;
    }

     void OnCollisionEnter2D(Collision2D collision)
    {
        _spriteRenderer.sprite = _defaultSprite;
    }
}
