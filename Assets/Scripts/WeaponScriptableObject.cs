using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon", order = 1)]
public class WeaponScriptableObject : ScriptableObject
{

    [Header("Cost and Unlock")]
    public int cost;
    public bool unlocked = false;
    public bool unlockedByDefault = false;



    [Header("Stats")]
    public bool singleShot = false;
    public FloatUpgradableValue fireRate;
    public IntUpgradableValue ammoPerMagazine;
    public FloatUpgradableValue damage;
    public FloatUpgradableValue reloadSpeed;
    public IntUpgradableValue penetration;

    [Header("Other")]
    public AudioClip shotClip;
    public AudioClip reloadClip;
    public float volumeShot = 1f;
    public float volumeReload = 1f;

    

    public void ResetWeapon()
    {
        if (!unlockedByDefault)
            unlocked = false;
        else
            unlocked = true;

        fireRate.ResetProgress();
        ammoPerMagazine.ResetProgress();
        damage.ResetProgress();
        reloadSpeed.ResetProgress();
        penetration.ResetProgress();
    }
}