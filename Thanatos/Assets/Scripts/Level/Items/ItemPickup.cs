using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickupableWeapon
{
    public string Name { get; }
    public float Damage { get; }
    public void Pickup();
}

public interface IPickupableAttire
{
    public string Name { get; }
    public float EnemyDamageModifier { get; }
    public float SpeedModifier { get; }
    public void Pickup();
}

public interface IPickupableBonus
{
    public string Name { get; }
    public ParticleSystem PickupEffect { get; }
    public void Pickup();
}