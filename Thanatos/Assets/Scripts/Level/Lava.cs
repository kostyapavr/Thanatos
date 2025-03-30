using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        Player player = collision.GetComponent<Player>();
        if (player != null && !LevelController.playerHasLavaBoots)
        {
            LevelController.lavaBootsHP--;
            if (LevelController.lavaBootsHP <= 0)
            {
                player.RemoveLavaBoots();
                LevelController.playerHasLavaBoots = false;
            }
            player.TakeDamage(0.25f);
            player.GetComponent<PlayerMovement>().SlowDown();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision == null) return;
        Player player = collision.GetComponent<Player>();
        if (player != null && !LevelController.playerHasLavaBoots)
        {
            player.TakeDamage(0.1f * Time.deltaTime);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == null) return;
        PlayerMovement player = collision.GetComponent<PlayerMovement>();
        if (player != null)
        {
            player.ReturnToNormalSpeed();
        }
    }
}
