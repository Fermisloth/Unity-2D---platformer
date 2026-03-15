using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
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
        rb.linearVelocity = new Vector2(horizontal, rb.linearVelocity.y);
    }   

}
