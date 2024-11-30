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

    private void Shoot()
    {
        if (Vector2.Distance(player.transform.position, transform.position) > 70) return;

        Vector2 diff = player.transform.position - transform.position;
        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        float spreadAngle = angle + Random.Range(-arrowSpread, arrowSpread + 1);
        Vector2 direction = Quaternion.AngleAxis(spreadAngle - angle, Vector3.forward) * diff.normalized;

        if (!spriteRenderer.flipX && direction.x < 0)
            spriteRenderer.flipX = true;
        else if (spriteRenderer.flipX && direction.x > 0)
            spriteRenderer.flipX = false;

        GameObject arrow = Instantiate(arrowPref, transform.position, Quaternion.Euler(0, 0, spreadAngle + 270));
        arrow.GetComponent<Arrow>().owner = "Enemy";
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
