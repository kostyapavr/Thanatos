using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint;
    public LayerMask enemyLayers;

    public int attackDamage = 30;
    public float attackRange = 0.5f;

    private ItemManager itemManager;

    private void Start()
    {
        itemManager = GetComponent<ItemManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && itemManager.hasSword)
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

