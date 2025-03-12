using System.Collections;
using System.Collections.Generic;
using System.Data;
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

    public virtual void Start()
    {
        GetComponent<Collider2D>().enabled = false;
        if (OwnerType == "Enemy") Invoke("EnableCollider", 0.1f);
        else Invoke("EnableCollider", 0.025f);
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
        IDamageable damageable = collision.collider.GetComponent<IDamageable>();
        if (OwnerType == "Player" && collision.collider.tag == "Player") return;
        if (OwnerID == collision.collider.gameObject.GetInstanceID()) return;
        if (damageable != null && collision.collider.tag != OwnerType)
        {
            damageable.TakeDamage(Damage, gameObject);
        }
        Destroy();
    }
}
