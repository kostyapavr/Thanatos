using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchillesHelmetPickup : MonoBehaviour, IPickupableAttire
{
    private string _name = "Achilles Helmet";
    private float enemyDamageModifier = 0.75f;
    private float speedModifier = 1.1f;

    private bool canInteract = false;
    public GameObject switchButton;
    public GameObject switchHelmetPrefab;

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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (canInteract) HideInteract();
        }
    }

    public void Pickup()
    {
        if (LevelController.playerHasAchillesHelmet) return;
        if (LevelController.playerHasHelmet)
        {
            DisplayInteract();
            return;
        }
        LevelController.helmetHP = LevelController.maxHelmetHP;
        LevelController.playerHasAchillesHelmet = true;
        LevelController.playerPickupItemEvent.Invoke();
        SaveItem(_name);
        Destroy(gameObject);
    }

    private void SwitchHelmets()
    {
        LevelController.helmetHP = LevelController.maxHelmetHP;
        LevelController.playerHasAchillesHelmet = true;
        LevelController.playerHasHelmet = false;
        LevelController.playerPickupItemEvent.Invoke();
        Instantiate(switchHelmetPrefab, transform.position, Quaternion.identity);
        SaveItem(switchHelmetPrefab.GetComponent<IPickupableAttire>().Name);
        Destroy(gameObject);
    }

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
                SwitchHelmets();
            }
        }
    }

    private void SaveItem(string n)
    {
        if (PlayerPrefs.GetInt(n, 0) == 0)
        {
            PlayerPrefs.SetInt(n, 1);
            PlayerPrefs.Save();
        }
    }
}
