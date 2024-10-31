using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 MovementSpeed = new Vector2(100.0f, 100.0f);
    private new Rigidbody2D rb;
    private Vector2 inputVector = new Vector2(0.0f, 0.0f);

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    void FixedUpdate()
    {
        rb.velocity += inputVector * 10;
    }
}