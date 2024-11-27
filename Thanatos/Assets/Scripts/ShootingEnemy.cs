using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : Enemy
{
    public new float maxHealth;
    public GameObject arrowPref;
    public Transform firePoint;
    public float shootInterval = 2;
    public int arrowSpeed;

    public override void Start()
    {
        currentHealth = maxHealth;
        InvokeRepeating("Shoot", shootInterval, shootInterval);
    }

    private void Shoot()
    {
        Vector3 direction = (FindObjectOfType<Player>().transform.position - firePoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        GameObject arrow = Instantiate(arrowPref, firePoint.position, Quaternion.Euler(0, 0, angle + 270));
        arrow.GetComponent<Arrow>().owner = "Enemy";
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.velocity = direction * arrowSpeed;
    }
}
