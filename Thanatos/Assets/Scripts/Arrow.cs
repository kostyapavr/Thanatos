using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    public float speed;
    public float distance;
    public int damage = 20;

    private void Update()
    {
        RaycastHit2D other = Physics2D.Raycast(transform.position, transform.up, distance);
        if (other.collider != null)
        {
            if (other.collider.CompareTag("Enemy"))
            {
                Destroy();
            }
        }

        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
