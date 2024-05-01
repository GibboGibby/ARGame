using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
struct WeaponStruct
{
    [SerializeField] public string name;
    [SerializeField] public WeaponScriptableObject weapon;
}
public class WeaponManager : MonoBehaviour
{
    [SerializeField] private List<WeaponStruct> weaponsPublic = new List<WeaponStruct>();

    private Dictionary<string, WeaponScriptableObject> weapons = new Dictionary<string, WeaponScriptableObject>();
    void Start()
    {
        foreach (WeaponStruct weapon in weaponsPublic)
        {
            weapons.Add(weapon.name, weapon.weapon);
        }
        
        if (!PlayerPrefs.HasKey("FirstTimeBoot"))
        {
            PlayerPrefs.SetInt("FirstTimeBoot", 0);
            foreach (WeaponStruct weapon in weaponsPublic)
            {
                weapon.weapon.ResetWeapon();
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log(weapons["revolver"].fireRate.GetAllUpgradesAndCosts());
            //weaponsPublic[0].weapon.fireRate.GetAllUpgradesAndCosts();
        }
    }
}
