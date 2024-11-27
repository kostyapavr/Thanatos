using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 inputVector = new Vector2(0.0f, 0.0f);
    [HideInInspector] public int acceleration;
    public Transform attackPoint;
    public Transform bow;

    private int slowAcceleration = 1;
    private int normalAcceleration = 12;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        acceleration = normalAcceleration;
    }

    void Update()
    {
        inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (inputVector.x < 0.0f)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            attackPoint.localPosition = new Vector3(-attackPoint.localPosition.x, attackPoint.localPosition.y, 0);
            //attackPoint.rotation = new Quaternion(attackPoint.rotation.x, 180, attackPoint.rotation.z, 0);
            bow.localPosition = new Vector3(-bow.localPosition.x, bow.localPosition.y, 0);
            bow.rotation = new Quaternion(bow.rotation.x, 180, bow.rotation.z, 0);
        }
        else if (inputVector.x > 0.0f)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            attackPoint.localPosition = new Vector3(-attackPoint.localPosition.x, attackPoint.localPosition.y, 0);
            //attackPoint.rotation = new Quaternion(attackPoint.rotation.x, 0, attackPoint.rotation.z, 0);
            bow.localPosition = new Vector3(-bow.localPosition.x, bow.localPosition.y, 0);
            bow.rotation = new Quaternion(bow.rotation.x, 0, bow.rotation.z, 0);
        }
    }

    void FixedUpdate()
    {
        rb.velocity += inputVector * acceleration;
    }

    public void SlowDown()
    {
        acceleration = slowAcceleration;
    }

    public void ReturnToNormalSpeed()
    {
        acceleration = normalAcceleration;
    }
}