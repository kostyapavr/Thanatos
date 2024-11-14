using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    public float healthAmount = 0.5f; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ���������, �������� �� ������ �������
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            player.AddHealth(healthAmount);
            Destroy(gameObject);
        }
    }
}