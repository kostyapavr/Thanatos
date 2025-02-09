using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmetPickup : MonoBehaviour, IPickupableAttire
{
    private string _name = "Helmet";
    private float enemyDamageModifier = 0.8f;
    private float speedModifier = 1.1f;
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
        LevelController.playerHasHelmet = true;
        LevelController.playerPickupItemEvent.Invoke();
        Destroy(gameObject);
    }
}
