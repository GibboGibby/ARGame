using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
struct WeaponStruct
{
    [SerializeField] public WeaponsEnum type;
    [SerializeField] public WeaponScriptableObject weapon;
}

enum WeaponsEnum
{
    Revolver,
    MachineGun,
    Shotgun
}
public class WeaponManager : MonoBehaviour
{
    [SerializeField] private List<WeaponStruct> weaponsPublic = new List<WeaponStruct>();

    private Dictionary<WeaponsEnum, WeaponScriptableObject> weapons = new Dictionary<WeaponsEnum, WeaponScriptableObject>();
    void Start()
    {
        foreach (WeaponStruct weapon in weaponsPublic)
        {
            weapons.Add(weapon.type, weapon.weapon);
        }
        
        if (!PlayerPrefs.HasKey("FirstTimeBoot"))
        {
            PlayerPrefs.SetInt("FirstTimeBoot", 0);
            GameManager.Instance.playerStats.ResetProgress();
            foreach (WeaponStruct weapon in weaponsPublic)
            {
                weapon.weapon.ResetWeapon();
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        #if !UNITY_EDITOR
        return;
        #endif
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log(weapons[WeaponsEnum.Revolver].fireRate.GetAllUpgradesAndCosts());
            Debug.Log(weapons[WeaponsEnum.Revolver].ammoPerMagazine.GetAllUpgradesAndCosts());
            //weaponsPublic[0].weapon.fireRate.GetAllUpgradesAndCosts();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log(weapons[WeaponsEnum.Revolver].fireRate.GetValue());
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            weapons[WeaponsEnum.Revolver].fireRate.UpgradeValue();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            PlayerPrefs.DeleteKey("FirstTimeBoot");
        }
    }
}
