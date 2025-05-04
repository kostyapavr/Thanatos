using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public class Arrow : MonoBehaviour, IShootable
{
    private string _name = "Arrow";
    [HideInInspector]
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }
    private float _damage = 1.0f;
    [HideInInspector]
    public float Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }

    protected string _ownerType = string.Empty;
    public string OwnerType
    {
        get { return _ownerType; }
        set { _ownerType = value; }
    }

    protected int _ownerID = 0;
    public int OwnerID
    {
        get { return _ownerID; }
        set { _ownerID = value; }
    }

    public ParticleSystem particles;

    [HideInInspector]
    public DamageEffects damageEffect = DamageEffects.Nothing;

    [HideInInspector] public bool explodeOnImpact = false;
    public GameObject explodeEffect;

    public virtual void Start()
    {
        GetComponent<Collider2D>().enabled = false;
        if (OwnerType == "Enemy") Invoke("EnableCollider", 0.15f);
        else Invoke("EnableCollider", 0.03f);
    }

    public void EnableCollider()
    {
        GetComponent<Collider2D>().enabled = true;
    }

    public virtual void Destroy()
    {
        Destroy(gameObject);
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        var pos = transform.position;
        IDamageable damageable = collision.collider.GetComponent<IDamageable>();
        if (OwnerType == "Player" && collision.collider.tag == "Player") return;
        if (OwnerID == collision.collider.gameObject.GetInstanceID()) return;

        GetComponent<Collider2D>().enabled = false;
        if (damageable != null && collision.collider.tag != OwnerType)
        {
            damageable.TakeDamage(Damage, gameObject, damageEffect);
        }

        if (explodeOnImpact)
        {
            DoExplosion();
            Instantiate(explodeEffect, pos, Quaternion.identity);
        }

        GameObject g = Instantiate(particles.gameObject, pos, Quaternion.identity);
        Destroy(g, 1);
        Destroy();
    }

    private void DoExplosion()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 7f, transform.position, 0);
        if (hits.Where(x => x.collider != null && x.collider.GetComponent<Enemy>()).Count() > 0)
        {
            hits.First(x => x.collider.GetComponent<Enemy>()).collider.GetComponent<Enemy>().TakeDamage(0.5f, gameObject, DamageEffects.Confusion);
        }
    }
}
