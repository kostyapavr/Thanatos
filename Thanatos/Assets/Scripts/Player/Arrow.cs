using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float damage = 0.5f;

    public bool playerArrow = true;

    void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable damageable = collision.collider.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage, gameObject);
        }
        Destroy();
    }
}
