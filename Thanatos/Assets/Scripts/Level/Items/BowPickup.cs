using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowPickup : MonoBehaviour, IPickupableWeapon
{
    [SerializeField]
    private string _name;
    [SerializeField]
    private float damage;
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
        if (LevelController.playerHasBow || LevelController.playerHasSword || LevelController.playerHasFireBow) return;
        LevelController.playerHasBow = true;
        LevelController.playerHasFireBow = false;
        LevelController.currentPlayerWeapon = this;
        LevelController.playerPickupItemEvent.Invoke();
        LevelController.playerWeapons.Add(this);
        Destroy(gameObject);
    }
}
