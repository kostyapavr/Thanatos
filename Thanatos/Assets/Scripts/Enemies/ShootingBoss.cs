using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingBoss : ShootingEnemy
{
    public GameObject hpBar;
    public Image hpBarFill;

    protected override void Shoot()
    {
        if (!PlayerInSight()) return;

        Vector2 diff = player.transform.position - transform.position;
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
        arrow.GetComponent<Arrow>().Damage = 1.0f;
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * ammoSpeed + GetComponent<Rigidbody2D>().velocity, ForceMode2D.Impulse);
    }

    protected override void ShowDamage()
    {
        if (!hpBar.activeSelf) hpBar.SetActive(true);
        hpBarFill.fillAmount = currentHealth / health;
        base.ShowDamage();
    }
}
