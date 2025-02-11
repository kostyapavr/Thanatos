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
    
    public int arrowSpeedMin;
    public int arrowSpeedMax;

    public int arrowSpread;

    public bool enemyIsBoss = false;
    public GameObject hpBar;
    public Image hpBarFill;

    public GameObject arrowPref;

    public float shootingRange;
    public LayerMask PlayerMask;

    private Player player;
    private SpriteRenderer spriteRenderer;
    private float health;
    private float shootInterval;
    private int arrowSpeed;

    public override void Start()
    {
        RandomiseProperties();
        isBoss = enemyIsBoss;

        currentHealth = health;
        player = FindObjectOfType<Player>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        InvokeRepeating("Shoot", shootInterval, shootInterval);
    }

    private bool PlayerInSight()
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

    private void Shoot()
    {
        if (!PlayerInSight()) return;

        Vector2 diff = player.transform.position - transform.position;
        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        float spreadAngle = angle + Random.Range(-arrowSpread, arrowSpread + 1);
        Vector2 direction = Quaternion.AngleAxis(spreadAngle - angle, Vector3.forward) * diff.normalized;

        if (!spriteRenderer.flipX && direction.x < 0)
            spriteRenderer.flipX = true;
        else if (spriteRenderer.flipX && direction.x > 0)
            spriteRenderer.flipX = false;

        GameObject arrow = Instantiate(arrowPref, transform.position, Quaternion.Euler(0, 0, spreadAngle + 270));
        arrow.GetComponent<Arrow>().ownerType = "Enemy";
        arrow.GetComponent<Arrow>().ownerID = gameObject.GetInstanceID();
        if (isBoss) arrow.GetComponent<Arrow>().SetBossDamage();
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * arrowSpeed + GetComponent<Rigidbody2D>().velocity, ForceMode2D.Impulse);
    }

    private void RandomiseProperties()
    {
        health = Random.Range(minHealth, maxHealth);
        shootInterval = Random.Range(shootIntervalMin, shootIntervalMax);
        arrowSpeed = Random.Range(arrowSpeedMin, arrowSpeedMax);
    }

    protected override void ShowDamage()
    {
        if (isBoss)
        {
            if (!hpBar.activeSelf) hpBar.SetActive(true);
            hpBarFill.fillAmount = currentHealth / health;
        }
        base.ShowDamage();
    }
}
