using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon", order = 1)]
public class WeaponScriptableObject : ScriptableObject
{
    public FloatUpgradableValue fireRate;
    public IntUpgradableValue ammoPerMagazine;
}