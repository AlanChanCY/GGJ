using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/ShipData", order = 1)]
public class ShipData : ScriptableObject
{
    // Generic Ship Stuff that is important to have
    public int health;
    public int fireRate;
    public int speed;
    public int damagePerShot;
    public int cost;
    public float range;
}
