using System;
using UnityEngine;
using UnityEngine.Events;


public static class PlayerState
{
    //weapon pickup
    //public delegate void PickupWeapon(WeaponClassname weaponType);
    //public static PickupWeapon OnWeaponPickup;
    //public static UnityEvent teleportEvent = new UnityEvent();
    //public static Vector3 teleportDestination = Vector3.zero;
    //public static Vector3 teleportAngles = Vector3.zero;

    //Player Movement State
    public static float pm_gravity = 1000;
    public static Vector3 currentPosition = Vector3.zero;
    public static Vector3 currentVelocity = Vector3.zero;
    public static PMFlags moveFlags;
    public static float currentSpeed = 0;
    //public static Vector3 addVelocities = Vector3.zero;
    //public static float currentViewHeight = 0;

    //Player Health
    public static float playerEnergy = 60;

    //Spawn
    //public static Vector3 spawnOrigin = Vector3.zero;
    //public static Vector3 spawnAngles = Vector3.right;

}
