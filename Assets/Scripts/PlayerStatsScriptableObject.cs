using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/PlayerStats", order = 1)]
public class PlayerStatsScriptableObject : ScriptableObject
{
    //public int MaxHealth = 3;
    public IntUpgradableValue MaxHealth;
    public int Points = 0;

    [SerializeField] private WeaponScriptableObject startingWeapon;

    public WeaponScriptableObject currentWeapon;


    public void ResetProgress()
    {
        Points = 0;
        MaxHealth.ResetProgress();
        currentWeapon = startingWeapon;
    }
}
