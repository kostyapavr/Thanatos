using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float distance;
    public int damage = 20;

    void Destroy()
    {
        Destroy(gameObject);
    }

    void DamageEnemy(Enemy enemy, int dmg)
    {
        enemy.TakeDamage(damage);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            DamageEnemy(collision.collider.GetComponent<Enemy>(), damage);
            Destroy();
        }
        else if (!collision.collider.CompareTag("Player")) Destroy();
    }
}
