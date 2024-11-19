using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : Enemy
{
    public float moveInterval = 5;
    public float moveSpeed = 5;
    public float moveDistance = 5;

    private Rigidbody2D rb;

    public override void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("MoveRandomly", moveInterval, moveInterval);
    }

    private void MoveRandomly()
    {
        Vector2 direction = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)).normalized;

        float distance = Random.Range(moveDistance / 2, moveDistance);

        rb.velocity = direction * moveSpeed;

        Invoke("StopMoving", distance / moveSpeed);
    }

    private void StopMoving()
    {
        rb.velocity = Vector2.zero;
    }
}