using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPickup : MonoBehaviour, IPickupableWeapon
{
    private string _name = "Sword";
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
        LevelController.playerHasSword = true;
        LevelController.playerSelectedWeapon = 1;
        LevelController.playerPickupItemEvent.Invoke();
        Destroy(gameObject);
    }
}
