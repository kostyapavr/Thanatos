using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : Enemy
{
    public GameObject arrowPref;
    public Transform firePoint;
    public float shootInterval = 2;

    public override void Start()
    {
        currentHealth = maxHealth;
        InvokeRepeating("Shoot", shootInterval, shootInterval);
    }

    private void Shoot()
    {
        GameObject arrow = Instantiate(arrowPref, firePoint.position, firePoint.rotation);
        arrow.GetComponent<Arrow>().playerArrow = false;
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.velocity = (FindObjectOfType<Player>().transform.position - firePoint.position).normalized * 10;
    }
}