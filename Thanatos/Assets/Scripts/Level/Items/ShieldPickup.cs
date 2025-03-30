using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickup : MonoBehaviour, IPickupableWeapon
{
    private bool canInteract = false;
    public GameObject switchButton;

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
            DisplayInteract();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            HideInteract();
        }
    }

    public void Pickup()
    {
        /*if (LevelController.playerHas)
        {
            DisplayInteract();
            return;
        }*/
        LevelController.playerHasShield = true;
        LevelController.playerWeapons.Add(this);
        LevelController.currentPlayerWeapon = this;
        LevelController.playerPickupItemEvent.Invoke();
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().SelectWeapon(this);
        Destroy(gameObject);
    }

    /*private void SwitchShield()
    {
        LevelController.helmetHP = LevelController.maxHelmetHP;
        LevelController.playerHasAchillesHelmet = false;
        LevelController.playerHasHelmet = true;
        LevelController.playerPickupItemEvent.Invoke();
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().RemoveAchillesHelmet();
        Destroy(gameObject);
    }*/

    void DisplayInteract()
    {
        canInteract = true;
        switchButton.SetActive(true);
    }

    void HideInteract()
    {
        canInteract = false;
        switchButton.SetActive(false);
    }

    private void LateUpdate()
    {
        if (canInteract)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Pickup();
            }
        }
    }

    /*private void CheckSwitch()
    {
        var p = transform.position;
        if (LevelController.playerHasBow)
        {
            Instantiate(bowPrefab, p, Quaternion.identity);
        }
        if (LevelController.playerHasSword)
        {
            Instantiate(swordPrefab, p, Quaternion.identity);
        }
    }*/
}
