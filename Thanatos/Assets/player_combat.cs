using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class player_combat : MonoBehaviour
{
    public Transform attackPoint;
    public LayerMask enemyLayers;

    public int attackDamage = 30;
    public float attackRange = 0.5f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Attack();
        }
    }

    void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position,attackRange,enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }

    } 
}

