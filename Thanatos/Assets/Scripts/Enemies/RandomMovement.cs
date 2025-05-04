using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcherMovement : MonoBehaviour
{
    public float moveIntervalMin;
    public float moveIntervalMax;
    public float moveSpeed;
    public float moveDistance;
    public float obstacleDetectSize;
    public float detectionDistance;

    private Rigidbody2D rb;
    private Transform playerTransform;
    private int obstacleMask;
    private ShootingEnemy shootingEnemyScript;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        shootingEnemyScript = GetComponent<ShootingEnemy>();
        obstacleMask = LayerMask.GetMask("Obstacle");
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating("MoveTowardsPlayer", moveIntervalMin, Random.Range(moveIntervalMin, moveIntervalMax));
    }

    private void MoveTowardsPlayer()
    {
        if (shootingEnemyScript.freeze)
        {
            shootingEnemyScript.freeze = false;
            return;
        }

        Vector2 direction = GetDirectionToPlayer();

        RaycastHit2D hit = Physics2D.CircleCast(transform.position, obstacleDetectSize, direction, detectionDistance, obstacleMask);
        if (hit.collider != null && !hit.collider.GetComponent<Player>())
        {
            direction = direction + Vector2.Perpendicular(direction);
            //transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + newDirection, moveSpeed * Time.deltaTime);
        }

        float distance = Random.Range(moveDistance / 1.5f, moveDistance);
        rb.velocity = direction * moveSpeed;
        Invoke("StopMoving", distance / moveSpeed);
    }

    private void StopMoving()
    {
        rb.velocity = Vector2.zero;
    }

    Vector2 GetDirectionToPlayer()
    {
        if (playerTransform == null)
            return Vector2.zero;

        return (playerTransform.position - transform.position).normalized;
    }
}