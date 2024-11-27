using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : Enemy
{
    public float moveIntervalMin = 2.0f;
    public float moveIntervalMax = 5.0f;
    public float moveSpeed = 5;
    public float moveDistance = 5;

    private Rigidbody2D rb;

    private int obstacleMask;

    public override void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        obstacleMask = LayerMask.GetMask("Obstacle");
        InvokeRepeating("MoveRandomly", moveIntervalMin, Random.Range(moveIntervalMin, moveIntervalMax));
    }

    private void MoveRandomly()
    {
        Vector2 direction = GenDirection();

        float distance = Random.Range(moveDistance / 2, moveDistance);

        rb.velocity = direction * moveSpeed;

        Invoke("StopMoving", distance / moveSpeed);
    }

    private void StopMoving()
    {
        rb.velocity = Vector2.zero;
    }

    Vector2 GenDirection()
    {
        Vector2 direction = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, direction, 15, obstacleMask);
        if (!hit)
        {
            //Debug.DrawRay(transform.position, direction * 15, Color.green, 2);
            return direction;
        }

        int iter = 0;
        while (hit.collider != null)
        {
            if (iter >= 10) break;
            direction = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
            hit = Physics2D.Raycast((Vector2)transform.position, direction, 15, obstacleMask);
            //Debug.DrawRay(transform.position, direction * 15, Color.red, 2);
            iter++;
        }
        //Debug.DrawRay(transform.position, direction * 15, Color.green, 2);
        return direction;
    }
}