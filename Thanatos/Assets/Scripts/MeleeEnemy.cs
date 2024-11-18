using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    public Transform attackPoint;
    public float attackInterval = 2;

    public override void Start()
    {
        currentHealth = maxHealth;
        InvokeRepeating("Attack", attackInterval, attackInterval);
    }

    private void Attack()
    {
        
    }

    public override void FixedUpdate()
    {
        ChasePlayer();
    }

    private void ChasePlayer()
    {

    }
}
