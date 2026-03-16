using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float jumpEndTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        Debug.Log(horizontal);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        var vertiacal = rb.linearVelocity.y;

        if (Input.GetButtonDown("Fire1"))

            jumpEndTime = Time.time + 0.5f;
                    
                
        if (Input.GetButton("Fire1") && jumpEndTime > Time.time)
            
            vertiacal = 5;

        rb.linearVelocity = new Vector2(horizontal, vertiacal);
    }   

}
