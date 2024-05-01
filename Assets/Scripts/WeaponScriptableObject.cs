using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon", order = 1)]
public class WeaponScriptableObject : ScriptableObject
{
    public float cost;
    public bool unlocked = false;
    public bool unlockedByDefault = false;

    public FloatUpgradableValue fireRate;
    public IntUpgradableValue ammoPerMagazine;

    public void ResetWeapon()
    {
        if (!unlockedByDefault)
            unlocked = false;
        else
            unlocked = true;
    }
}