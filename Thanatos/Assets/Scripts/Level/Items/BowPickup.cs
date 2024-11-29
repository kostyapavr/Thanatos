using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowPickup : MonoBehaviour, IPickupable
{
    private string _name = "Bow";
    public string Name {  get => _name; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Pickup();
        }
    }

    public void Pickup()
    {
        LevelController.playerHasBow = true;
        LevelController.playerPickupItemEvent.Invoke();
        Destroy(gameObject);
    }
}
