using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/AllyData", order = 2)]
public class AllyData : ScriptableObject
{
    // Generic Ship Stuff that is important to have
    public int health;
    public int fireRate;
    public int speed;
    public int damagePerShot;
    public int cost;
    public float range;

    // flocking multipliers
    public float cohesionMult;
    public float velocityMult;
    public float avoidanceMult;
    public float groupAvoidanceMult;
}
