using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fireball : MonoBehaviour, IShootable
{
    private string _name = "Fireball";
    [HideInInspector]
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }
    private float _damage = 0.5f;
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

    public virtual void Start()
    {
        GetComponent<Collider2D>().enabled = false;
        if (OwnerType == "Enemy") Invoke("EnableCollider", 0.15f);
        else Invoke("EnableCollider", 0.025f);
    }

    public void EnableCollider()
    {
        GetComponent<Collider2D>().enabled = true;
    }

    public virtual void Destroy()
    {
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        particles.Clear();
        particles.Stop();
        particles.Play();

        Invoke("DeleteFireball", particles.main.duration);
    }

    private void DoAoA_Damage()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 4f, transform.position);
        if (hits.Where(x => x.collider != null && x.collider.GetComponent<Player>()).Count() > 0)
        {
            hits.First(x => x.collider.GetComponent<Player>()).collider.GetComponent<Player>().TakeDamage(0.5f, gameObject);
        }
    }

    private void DeleteFireball()
    {
        Destroy(gameObject);
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable damageable = collision.collider.GetComponent<IDamageable>();
        if (OwnerType == "Player" && collision.collider.tag == "Player") 
		{
			Destroy();
			return;
		}
        if (OwnerID == collision.collider.gameObject.GetInstanceID()) 
		{
			Destroy();
			return;
		}
		
        if (damageable != null && collision.collider.tag != OwnerType)
        {
            damageable.TakeDamage(Damage, gameObject);
            Destroy();
        }
        else
        {
            DoAoA_Damage();
            Destroy();
        }
    }
}
