using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingBoss : ShootingEnemy
{
    public GameObject hpBar;
    public Image hpBarFill;
    public Transform fireballGrowPosition;

    public GameObject fireballGrow;

    private GameObject fb_grow;
    bool growFireball = false;

    protected override void Shoot()
    {
        if (!PlayerInSight()) return;

        Vector2 diff = player.transform.position - fireballGrowPosition.position;
        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        float spreadAngle = angle + Random.Range(-ammoSpread, ammoSpread + 1);
        Vector2 direction = Quaternion.AngleAxis(spreadAngle - angle, Vector3.forward) * diff.normalized;

        if (!spriteRenderer.flipX && direction.x < 0)
        {
            fireballGrowPosition.localPosition = new Vector3(-fireballGrowPosition.localPosition.x, fireballGrowPosition.localPosition.y, 0);
            if (fb_grow != null) fb_grow.transform.localPosition = new Vector3(-fb_grow.transform.localPosition.x, fb_grow.transform.localPosition.y, 0);
            spriteRenderer.flipX = true;
        }
        else if (spriteRenderer.flipX && direction.x > 0)
        {
            fireballGrowPosition.localPosition = new Vector3(-fireballGrowPosition.localPosition.x, fireballGrowPosition.localPosition.y, 0);
            if (fb_grow != null) fb_grow.transform.localPosition = new Vector3(-fb_grow.transform.localPosition.x, fb_grow.transform.localPosition.y, 0);
            spriteRenderer.flipX = false;
        }

        GameObject fireball = Instantiate(ammoPrefab, fireballGrowPosition.position, Quaternion.Euler(0, 0, spreadAngle + 270));
        fireball.GetComponent<Fireball>().OwnerType = "Enemy";
        fireball.GetComponent<Fireball>().OwnerID = gameObject.GetInstanceID();
        fireball.GetComponent<Fireball>().Damage = 1.0f;
        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * ammoSpeed + GetComponent<Rigidbody2D>().velocity, ForceMode2D.Impulse);
        growFireball = false;
    }

    protected override void ShowDamage()
    {
        if (!hpBar.activeSelf) hpBar.SetActive(true);
        hpBarFill.fillAmount = currentHealth / health;
        base.ShowDamage();
    }

    public override void FixedUpdate()
    {
        if (growFireball && fb_grow != null)
        {
            if (fb_grow.transform.localScale.x < 0.19f)
                fb_grow.transform.localScale += Vector3.one * Time.deltaTime * 0.1f;
        }
        else
        {
            if (fb_grow == null)
                fb_grow = Instantiate(fireballGrow, fireballGrowPosition.position, Quaternion.identity, transform);
            fb_grow.transform.localScale = Vector3.zero;
            growFireball = true;
        }
    }
}
