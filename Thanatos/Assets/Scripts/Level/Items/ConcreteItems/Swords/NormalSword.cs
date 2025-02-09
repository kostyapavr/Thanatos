using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalSword : SwordPickup
{
    private string _name = "Metal Sword";
    private float damage = 1.0f;

    public new string Name { get => _name; }

    public new float Damage { get => damage; }

    public override void Pickup()
    {
        LevelController.currentPlayerWeapon = this;
        base.Pickup();
    }
}
