using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowPickup : MonoBehaviour, IPickupableWeapon
{
    [SerializeField]
    private string _name;
    [SerializeField]
    private float damage;
    public string Name { get => _name; }

    public float Damage { get => damage; }

    public GameObject interactButton;
    private bool canInteract = false;
    public GameObject swordPrefab;
    public GameObject peleusSwordPrefab;
    public GameObject apolloBowPrefab;
    public GameObject fireBowPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ShowInteract();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            HideInteract();
        }
    }

    public virtual void Pickup()
    {
        LevelController.playerHasBow = true;
        CheckSwitch();
        LevelController.playerHasSword = false;
        LevelController.playerHasApolloBow = false;
        LevelController.playerHasPeleusSword = false;
        LevelController.playerHasFireBow = false;
        LevelController.currentPlayerWeapon = this;
        LevelController.playerPickupItemEvent.Invoke();
        LevelController.playerWeapons.Add(this);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().SelectWeapon(this);
        Destroy(gameObject);
    }

    void ShowInteract()
    {
        interactButton.SetActive(true);
        canInteract = true;
    }

    void HideInteract()
    {
        interactButton.SetActive(false);
        canInteract = false;
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

    private void CheckSwitch()
    {
        var p = transform.position;
        if (LevelController.playerHasSword)
        {
            Instantiate(swordPrefab, p, Quaternion.identity);
        }
        if (LevelController.playerHasFireBow)
        {
            Instantiate(fireBowPrefab, p, Quaternion.identity);
        }
        if (LevelController.playerHasPeleusSword)
        {
            Instantiate(peleusSwordPrefab, p, Quaternion.identity);
        }
        if (LevelController.playerHasApolloBow)
        {
            Instantiate(apolloBowPrefab, p, Quaternion.identity);
        }
    }
}
