using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApolloBowPickup : MonoBehaviour, IPickupableWeapon
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
    public GameObject bowPrefab;
    public GameObject fireBowPrefab;
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
        PlayerPrefs.SetInt("HasApolloBow", 1);
        PlayerPrefs.Save();

        LevelController.playerHasApolloBow = true;
        CheckSwitch();
        LevelController.playerHasBow = false;
        LevelController.playerHasErosBow = false;
        LevelController.playerHasFireBow = false;
        LevelController.currentPlayerWeapon = this;
        LevelController.playerWeapons.Add(this);
        LevelController.playerPickupItemEvent.Invoke();
        if (LevelController.playerHasPeleusSword) GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().RemovePeleusSword();
        if (LevelController.playerHasSword) GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().RemoveDefaultSword();
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().SelectWeapon(this);
        Destroy(gameObject);
    }

    private void Start()
    {
        //if (PlayerPrefs.GetInt("HasApolloBow") == 0 && !SceneManager.GetActiveScene().name.Contains("Boss")) gameObject.SetActive(false);
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
        if (LevelController.playerHasSword)
        {
            Instantiate(swordPrefab, p, Quaternion.identity);
        }
        if (LevelController.playerHasPeleusSword)
        {
            Instantiate(peleusSwordPrefab, p, Quaternion.identity);
        }
        if (LevelController.playerHasFireBow)
        {
            Instantiate(fireBowPrefab, p, Quaternion.identity);
        }
        if (LevelController.playerHasErosBow)
        {
            Instantiate(erosBowPrefab, p, Quaternion.identity);
        }
    }
}
