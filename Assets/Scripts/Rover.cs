using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rover : MonoBehaviour
{
    float fuel = 100f;
    float hullHealth = 100f;
    float wheelHealth = 100f;
    float ammo = 100f;

    public enum roverStatus
    {
        defending   = 1,
        moving = 2,
        scanning = 3
    }

    
    
}
