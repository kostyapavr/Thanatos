using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puddle : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        PlayerMovement player = collision.GetComponent<PlayerMovement>();
        if (player != null)
        {
            player.PuddleSlowDown();
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
