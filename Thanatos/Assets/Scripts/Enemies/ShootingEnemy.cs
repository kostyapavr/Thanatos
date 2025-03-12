using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingEnemy : Enemy
{
    public float minHealth;
    public new float maxHealth;

    public float shootIntervalMin;
    public float shootIntervalMax;
    
    public int ammoSpeedMin;
    public int ammoSpeedMax;

    public int ammoSpread;

    public GameObject ammoPrefab;

    public float shootingRange;
    public LayerMask PlayerMask;

    public bool shootAhead;

    protected Player player;
    protected SpriteRenderer spriteRenderer;

    protected float health;
    protected float shootInterval;
    protected int ammoSpeed;

    public override void Start()
    {
        RandomiseProperties();

        currentHealth = health;
        player = FindObjectOfType<Player>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        InvokeRepeating("Shoot", shootInterval, shootInterval);
    }

    protected bool PlayerInSight()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= shootingRange)
        {
            Vector2 directionToPlayer = (player.transform.position - transform.position);
            
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, shootingRange, PlayerMask);
            
            if (hit.collider.GetComponent<Player>()) return true;
        }
        return false;
    }

    protected virtual void Shoot()
    {
        if (!PlayerInSight()) return;

        Vector2 diff = player.transform.position - transform.position;
        if (shootAhead) diff = ((Vector2)player.transform.position + player.GetComponent<Rigidbody2D>().velocity*0.25f) - (Vector2)transform.position;
        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        float spreadAngle = angle + Random.Range(-ammoSpread, ammoSpread + 1);
        Vector2 direction = Quaternion.AngleAxis(spreadAngle - angle, Vector3.forward) * diff.normalized;

        if (!spriteRenderer.flipX && direction.x < 0)
            spriteRenderer.flipX = true;
        else if (spriteRenderer.flipX && direction.x > 0)
            spriteRenderer.flipX = false;

        GameObject arrow = Instantiate(ammoPrefab, transform.position, Quaternion.Euler(0, 0, spreadAngle + 270));
        arrow.GetComponent<Arrow>().OwnerType = "Enemy";
        arrow.GetComponent<Arrow>().OwnerID = gameObject.GetInstanceID();
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * ammoSpeed + GetComponent<Rigidbody2D>().velocity, ForceMode2D.Impulse);
    }

    protected virtual void RandomiseProperties()
    {
        health = Random.Range(minHealth, maxHealth);
        shootInterval = Random.Range(shootIntervalMin, shootIntervalMax);
        ammoSpeed = Random.Range(ammoSpeedMin, ammoSpeedMax);
    }
}
