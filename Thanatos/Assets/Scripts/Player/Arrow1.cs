using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow1 : MonoBehaviour
{
    public float damage = 0.5f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Player player = collision.collider.GetComponent<Player>();
            if (player != null)
            {
                Debug.Log(collision.gameObject.name);
                player.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
