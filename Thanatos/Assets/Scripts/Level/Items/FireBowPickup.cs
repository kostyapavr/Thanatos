using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FireBowPickup : MonoBehaviour, IPickupableWeapon
{
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
            Pickup();
        }
    }

    public virtual void Pickup()
    {
        PlayerPrefs.SetInt("HasFireBow", 1);
        PlayerPrefs.Save();
        if (LevelController.playerHasBow && SceneManager.GetActiveScene().buildIndex == 1) return;
        LevelController.playerHasBow = false;
        LevelController.playerHasFireBow = true;
        LevelController.currentPlayerWeapon = this;
        LevelController.playerWeapons.Add(this);
        LevelController.playerPickupItemEvent.Invoke();
        Destroy(gameObject);
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("HasFireBow") == 0 && SceneManager.GetActiveScene().buildIndex != 16) gameObject.SetActive(false);
    }
}
