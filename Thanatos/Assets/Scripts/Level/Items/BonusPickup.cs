using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusPickup : MonoBehaviour, IPickupableBonus
{
    [SerializeField]
    private string _name;

    [SerializeField]
    private ParticleSystem pickupEffect;

    public BonusTypes bonusType;
    public string Name { get => _name; }

    public ParticleSystem PickupEffect { get => pickupEffect; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (pickupEffect != null) Instantiate(pickupEffect, collision.transform);
            Pickup();
        }
    }

    public virtual void Pickup()
    {
        LevelController.ApplyBonus(bonusType);
        LevelController.playerPickupItemEvent.Invoke();
        Destroy(gameObject);
    }
}


public enum BonusTypes { None, Baytulus, Moli, Cornucopia, Garnet, Panacea, Miasma, FlaskOfIchor, Ambrosia, ContentionApple, AriadneThread, Omphalos }