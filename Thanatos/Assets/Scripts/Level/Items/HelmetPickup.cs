using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmetPickup : MonoBehaviour, IPickupable
{
    private string _name = "Helmet";
    public string Name { get => _name; }

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
        Destroy(gameObject);
    }
}
