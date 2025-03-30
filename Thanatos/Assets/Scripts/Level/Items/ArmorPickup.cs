using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorPickup : MonoBehaviour, IPickupableAttire
{
    private string _name = "The Golden Armor of Glaucus";
    private float enemyDamageModifier = 0.5f;
    private float speedModifier = 0.9f;

    private bool canInteract = false;
    public GameObject switchButton;
    public GameObject switchArmorPrefab;
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
        if (LevelController.playerHasArmor) return;
        if (LevelController.playerHasAchillesArmor)
        {
            DisplayInteract();
            return;
        }
        LevelController.armorHP = LevelController.maxArmorHP;
        LevelController.playerHasArmor = true;
        LevelController.playerPickupItemEvent.Invoke();
        SaveItem(_name);
        Destroy(gameObject);
    }

    private void SwitchArmor()
    {
        LevelController.armorHP = LevelController.maxArmorHP;
        LevelController.playerHasAchillesArmor = false;
        LevelController.playerHasArmor = true;
        LevelController.playerPickupItemEvent.Invoke();
        Player p = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        p.RemoveAchillesArmor();
        p.EquipArmor();
        Instantiate(switchArmorPrefab, transform.position, Quaternion.identity);
        SaveItem(switchArmorPrefab.GetComponent<IPickupableAttire>().Name);
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
                SwitchArmor();
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
