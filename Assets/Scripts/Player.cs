using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float jumpEndTime;
    [SerializeField] private float _horizontalvelocity = 3;
    [SerializeField] private float _jumpvelocity = 5;
    [SerializeField] private float _jumpduraion = 0.5f;
    public bool IsGround;

    void OnDrawGizmos()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - spriteRenderer.bounds.extents.y);
        Gizmos.color = Color.red;
        Gizmos.DrawLine (origin, origin + Vector2.down * 0.1f);
    }

    // Update is called once per fram
    void Update()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - spriteRenderer.bounds.extents.y);
        var hit = Physics2D.Raycast(origin, Vector2.down, 0.1f);
        if (hit.collider)
            IsGround = true;
        else
            IsGround = false;
        
        var horizontal = Input.GetAxis("Horizontal");
        Debug.Log(horizontal);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        var vertiacal = rb.linearVelocity.y;

        if (Input.GetButtonDown("Fire1") && IsGround)

            jumpEndTime = Time.time + _jumpduraion;
                    
                
        if (Input.GetButton("Fire1") && jumpEndTime > Time.time)
            
            vertiacal = _jumpvelocity;
        horizontal *= _horizontalvelocity;
        rb.linearVelocity = new Vector2(horizontal, vertiacal);
    }   

}
