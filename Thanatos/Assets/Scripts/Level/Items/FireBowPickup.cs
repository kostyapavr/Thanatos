using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (LevelController.playerHasBow) return;
        PlayerPrefs.SetInt("HasFireBow", 1);
        PlayerPrefs.Save();
        LevelController.playerHasBow = true;
        LevelController.currentPlayerWeapon = this;
        LevelController.playerPickupItemEvent.Invoke();
        LevelController.playerWeapons.Add(this);
        Destroy(gameObject);
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("HasFireBow") == 0) gameObject.SetActive(false);
    }
}
