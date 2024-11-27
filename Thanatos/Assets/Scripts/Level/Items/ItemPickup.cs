using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickupable
{
    public string Name { get; }
    public void Pickup();
}
