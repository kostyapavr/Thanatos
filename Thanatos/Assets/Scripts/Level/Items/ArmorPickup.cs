using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorPickup : MonoBehaviour, IPickupableAttire
{
    private string _name = "The Golden Armor of Glaucus";
    private float enemyDamageModifier = 0.5f;
    private float speedModifier = 0.9f;
    public string Name { get => _name; }

    public float EnemyDamageModifier { get => enemyDamageModifier; }
    public float SpeedModifier { get => speedModifier; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Pickup();
        }
    }

    public void Pickup()
    {
        if (LevelController.playerHasArmor) return;
        LevelController.armorHP = 15;
        LevelController.playerHasArmor = true;
        LevelController.playerPickupItemEvent.Invoke();
        Destroy(gameObject);
    }
}
