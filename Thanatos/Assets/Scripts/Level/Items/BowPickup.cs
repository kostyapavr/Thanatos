using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowPickup : MonoBehaviour, IPickupableWeapon
{
    private string _name = "Bow";
    private float damage = 1.0f;
    public string Name { get => _name; }

    public float Damage { get => damage; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Pickup();
        }
    }

    public virtual void Pickup()
    {
        LevelController.playerHasBow = true;
        LevelController.playerSelectedWeapon = 0;
        LevelController.playerPickupItemEvent.Invoke();
        Destroy(gameObject);
    }
}
