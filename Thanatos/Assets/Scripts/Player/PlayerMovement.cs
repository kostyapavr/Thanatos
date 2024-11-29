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
    public Transform helmet;
    public Transform bowShootPos;

    private int slowAcceleration = 1;
    private int normalAcceleration = 8;
    private bool hasFlipped = false;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        acceleration = normalAcceleration;
    }

    void Update()
    {
        inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (inputVector.x < 0.0f && !hasFlipped)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            attackPoint.localPosition = new Vector3(-attackPoint.localPosition.x, attackPoint.localPosition.y, 0);
            //attackPoint.rotation = new Quaternion(attackPoint.rotation.x, 180, attackPoint.rotation.z, 0);
            bow.localPosition = new Vector3(-bow.localPosition.x, bow.localPosition.y, 0);

            helmet.localPosition = new Vector3(-helmet.localPosition.x, helmet.localPosition.y, 0);
            helmet.rotation = new Quaternion(helmet.rotation.x, 180, helmet.rotation.z, helmet.rotation.w);

            bowShootPos.localPosition = new Vector3(-bowShootPos.localPosition.x, bowShootPos.localPosition.y, 0);
            bowShootPos.GetComponent<SpriteRenderer>().flipY = true;

            //bow.rotation = new Quaternion(bow.rotation.x, 180, bow.rotation.z, bow.rotation.w);
            hasFlipped = true;
        }
        else if (inputVector.x > 0.0f && hasFlipped)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            attackPoint.localPosition = new Vector3(-attackPoint.localPosition.x, attackPoint.localPosition.y, 0);
            //attackPoint.rotation = new Quaternion(attackPoint.rotation.x, 0, attackPoint.rotation.z, 0);
            bow.localPosition = new Vector3(-bow.localPosition.x, bow.localPosition.y, 0);

            helmet.localPosition = new Vector3(-helmet.localPosition.x, helmet.localPosition.y, 0);
            helmet.rotation = new Quaternion(helmet.rotation.x, 0, helmet.rotation.z, helmet.rotation.w);

            //bowShootPos.rotation = new Quaternion(bowShootPos.rotation.x, 0, bowShootPos.rotation.z, bowShootPos.rotation.w);
            bowShootPos.localPosition = new Vector3(-bowShootPos.localPosition.x, bowShootPos.localPosition.y, 0);
            bowShootPos.GetComponent<SpriteRenderer>().flipY = false;

            //bow.rotation = new Quaternion(bow.rotation.x, 0, bow.rotation.z, bow.rotation.w);
            hasFlipped = false;
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