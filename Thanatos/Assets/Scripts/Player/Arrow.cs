using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float damage = 0.5f;
    [HideInInspector] public string owner;

    private void Start()
    {
        GetComponent<Collider2D>().enabled = false;
        if (owner == "Enemy") Invoke("EnableCollider", 0.1f);
        else Invoke("EnableCollider", 0.025f);
    }

    void EnableCollider()
    {
        GetComponent<Collider2D>().enabled = true;
    }

    void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable damageable = collision.collider.GetComponent<IDamageable>();
        if (owner == "Player" && collision.collider.tag == "Player") return;
        if (damageable != null && collision.collider.tag != owner)
        {
            damageable.TakeDamage(damage, gameObject);
        }
        Destroy();
    }

    public void BoostDamage(float additionalDamage)
    {
        damage += additionalDamage;
    }

    public void SetBossDamage(float dmg = 1.0f)
    {
        damage = dmg;
    }
}
