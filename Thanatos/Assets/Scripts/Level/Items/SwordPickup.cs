using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPickup : MonoBehaviour
{
    private string _name = "Sword";
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
        LevelController.playerHasSword = true;
        Destroy(gameObject);
    }
}