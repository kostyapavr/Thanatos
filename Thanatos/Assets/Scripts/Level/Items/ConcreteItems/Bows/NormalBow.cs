using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBow : BowPickup
{
    private string _name = "Wooden Bow";
    private float damage = 1.0f;

    public new string Name { get => _name; }

    public new float Damage { get => damage; }

    public override void Pickup()
    {
        LevelController.currentPlayerWeapon = this;
        base.Pickup();
    }
}
