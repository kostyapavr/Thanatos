using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShootable
{
    public string Name { get; }
    public float Damage { get; }
    [HideInInspector] public string OwnerType { get; set; }
    [HideInInspector] public int OwnerID { get; set; }
    public void OnCollisionEnter2D(Collision2D collision);
    public void EnableCollider();
    public void Destroy();
}
