using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusPickup : MonoBehaviour, IPickupableBonus
{
    [SerializeField]
    private string _name;

    [SerializeField]
    private GameObject pickupEffect;

    public BonusTypes bonusType;
    public string Name { get => _name; }

    public GameObject PickupEffect { get => pickupEffect; }
    public Material pickupEffectMaterial;

    public GameObject interactButton;
    bool canInteract = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ShowInteract();   
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        HideInteract();
    }

    public virtual void Pickup()
    {
        LevelController.ApplyBonus(bonusType);
        LevelController.playerPickupItemEvent.Invoke();
        SaveItem();
        Destroy(gameObject);
    }

    private void ShowInteract()
    {
        interactButton.SetActive(true);
        canInteract = true;
    }

    private void HideInteract()
    {
        interactButton.SetActive(false);
        canInteract = false;
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (canInteract)
            {
                if (pickupEffect != null)
                {
                    GameObject go = Instantiate(pickupEffect, GameObject.FindGameObjectWithTag("Player").transform);
                    ParticleSystemRenderer ps = go.GetComponent<ParticleSystem>().GetComponent<ParticleSystemRenderer>();
                    if (ps != null)
                    {
                        ps.material = pickupEffectMaterial;
                    }
                    Destroy(go, 2f);
                }
                Pickup();
            }
        }
    }

    private void SaveItem()
    {
        if (PlayerPrefs.GetInt(_name, 0) == 0)
        {
            PlayerPrefs.SetInt(_name, 1);
            PlayerPrefs.Save();
        }
    }
}

public enum BonusTypes { None, Baytulus, Moli, Cornucopia, Garnet, Panacea, Miasma, FlaskOfIchor, Ambrosia, ContentionApple, AriadneThread, Omphalos }