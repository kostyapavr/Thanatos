using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float distance;
    public int damageToEnemy = 20;
    public float damageToPlayer = 0.5f;

    public bool playerArrow = true;

    void Destroy()
    {
        Destroy(gameObject);
    }

    void DamageEnemy(Collision2D collision, int dmg)
    {
        ShootingEnemy enemy = collision.collider.GetComponent<ShootingEnemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(dmg);
        }
        else 
        {
            MeleeEnemy meleeEnemy = collision.collider.GetComponent<MeleeEnemy>();
            if (meleeEnemy != null) meleeEnemy.TakeDamage(dmg);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (playerArrow)//стрелял игрок
        {
            if (collision.collider.CompareTag("Enemy"))
            {
                DamageEnemy(collision, damageToEnemy);
                Destroy();
            }
            else Destroy();
        }
        else//стрелял враг
        {
            if (collision.collider.CompareTag("Player"))
            {
                Player player = collision.collider.GetComponent<Player>();
                if (player != null)
                {
                    player.TakeDamage(damageToPlayer);
                }
            }
            Destroy(gameObject);
        }
    }
}
