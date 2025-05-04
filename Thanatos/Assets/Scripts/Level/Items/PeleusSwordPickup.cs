using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PeleusSwordPickup : MonoBehaviour, IPickupableWeapon
{
    [SerializeField]
    private string _name;
    [SerializeField]
    private float damage;
    public string Name { get => _name; }

    public float Damage { get => damage; }

    public GameObject interactButton;
    private bool canInteract = false;
    public GameObject bowPrefab;
    public GameObject swordPrefab;
    public GameObject harpeSwordPrefab;
    public GameObject fireBowPrefab;
    public GameObject apolloBowPrefab;
    public GameObject erosBowPrefab;

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
        PlayerPrefs.SetInt("HasPeleusSword", 1);
        PlayerPrefs.Save();

        LevelController.playerHasPeleusSword = true;
        CheckSwitch();
        LevelController.playerHasBow = false;
        LevelController.playerHasApolloBow = false;
        LevelController.playerHasFireBow = false;
        LevelController.playerHasErosBow = false;
        LevelController.currentPlayerWeapon = this;
        LevelController.playerWeapons.Add(this);
        LevelController.playerPickupItemEvent.Invoke();
        if (LevelController.playerHasSword) GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().RemoveDefaultSword();
        if (LevelController.playerHasHarpeSword) GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().RemoveHarpeSword();
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().SelectWeapon(this);
        Destroy(gameObject);
    }

    private void Start()
    {
        //if (PlayerPrefs.GetInt("HasPeleusSword") == 0) gameObject.SetActive(false);
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
        if (LevelController.playerHasBow)
        {
            Instantiate(bowPrefab, p, Quaternion.identity);
        }
        if (LevelController.playerHasFireBow)
        {
            Instantiate(fireBowPrefab, p, Quaternion.identity);
        }
        if (LevelController.playerHasApolloBow)
        {
            Instantiate(apolloBowPrefab, p, Quaternion.identity);
        }
        if (LevelController.playerHasSword)
        {
            Instantiate(swordPrefab, p, Quaternion.identity);
        }
        if (LevelController.playerHasHarpeSword)
        {
            Instantiate(harpeSwordPrefab, p, Quaternion.identity);
        }
        if (LevelController.playerHasErosBow)
        {
            Instantiate(erosBowPrefab, p, Quaternion.identity);
        }
    }
}
