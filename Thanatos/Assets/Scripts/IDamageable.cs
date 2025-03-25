using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    abstract float currentHealth { get; set; }
    abstract float maxHealth { get; set; }
    public void TakeDamage(float damage, GameObject sender = null, DamageEffects damageEffect = DamageEffects.Nothing);
}

public enum DamageEffects { Nothing, SlowDown }
