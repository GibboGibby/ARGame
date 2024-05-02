using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPanel : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private WeaponsEnum weaponEnum;
    [SerializeField] private WeaponScriptableObject weapon;

    [SerializeField] private TextMeshProUGUI equippedText;
    [SerializeField] private TextMeshProUGUI costText;

    [SerializeField] private Button equip;
    [SerializeField] private Button buy;

    [SerializeField] private PlayerStatsScriptableObject playerStats;

    [SerializeField] private List<WeaponPanel> otherPanels = new List<WeaponPanel>();
    void Start()
    {
        
    }

    public bool WeaponUnlocked()
    {
        return weapon.unlocked;
    }

    private void OnEnable()
    {
        equippedText.gameObject.SetActive(false);
        costText.gameObject.SetActive(false);
        buy.gameObject.SetActive(false);
        equip.gameObject.SetActive(false);
        if (weapon.unlocked)
        {
            if (playerStats.currentWeapon == weapon)
            {
                equippedText.gameObject.SetActive(true);
            }
            else
            {
                equip.gameObject.SetActive(true);
            }
        }
        else
        {
            costText.gameObject.SetActive(true);
            costText.text = $"Cost - {weapon.cost}";
            buy.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UnequipWeapon()
    {
        if (equippedText.gameObject.activeInHierarchy)
        {
            equippedText.gameObject.SetActive(false);
            equip.gameObject.SetActive(true);
        }
    }

    public void SetWeaponAsActive()
    {
        //GameManager.Instance.ChangeWeapon(weaponEnum);
        playerStats.currentWeapon = weapon;
        equip.gameObject.SetActive(false);
        equippedText.gameObject.SetActive(true);
        for (int i = 0; i < otherPanels.Count; i++)
        {
            if (otherPanels[i] == this) continue;
            otherPanels[i].UnequipWeapon();
        }
    }

    public void BuyWeapon()
    {
        if (GameManager.Instance.playerStats.Points >= weapon.cost)
        {
            GameManager.Instance.playerStats.Points -= weapon.cost;
            weapon.unlocked = true;
            buy.gameObject.SetActive(false);
            costText.gameObject.SetActive(false);
            equip.gameObject.SetActive(true);
        }
    }
}
