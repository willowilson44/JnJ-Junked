using System;
using UnityEngine;
using UnityEngine.Events;

public enum GameMode
{
    flying, spectating, training, timeattack
}

public static class PlayerState
{
    //weapon pickup
    //public delegate void PickupWeapon(WeaponClassname weaponType);
    //public static PickupWeapon OnWeaponPickup;
    //public static UnityEvent itemResetEvent = new UnityEvent();

    //public static UnityEvent teleportEvent = new UnityEvent();
    //public static Vector3 teleportDestination = Vector3.zero;
    //public static Vector3 teleportAngles = Vector3.zero;

    //pmove state
    public static float currentSpeed = 0;
    public static float currentViewHeight = 0;
    public static Vector3 currentPosition = Vector3.zero;
    public static Vector3 mins = Vector3.zero;
    public static Vector3 maxs = Vector3.zero;
    public static float pm_gravity = 1000;
    public static Vector3 addVelocities = Vector3.zero;
    public static PMFlags moveFlags;

    //Spawn
    //public static Vector3 spawnOrigin = Vector3.zero;
    //public static Vector3 spawnAngles = Vector3.right;

}
