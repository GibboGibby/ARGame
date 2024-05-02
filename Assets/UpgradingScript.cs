using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradingScript<T> : MonoBehaviour
{

    [SerializeField] private UpgradeableValueScriptableObject<T> upgradingValue;
    [SerializeField] private PlayerStatsScriptableObject playerStats;
    [SerializeField] private WeaponPanel parentPanel;
    // Start is called before the first frame update

    [SerializeField] private TextMeshProUGUI costValue;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI nextValue;
    [SerializeField] private TextMeshProUGUI nextText;
    [SerializeField] private Button upgradeButton;

    [SerializeField] private TextMeshProUGUI value;

    private void Start()
    {
        
    }

    private void OnEnable()
    {

        if (upgradingValue.IsUnityNull() || upgradingValue == null) Destroy(this);

        UpdateValues();
    }

    public void UpgradeClicked()
    {
        if (GameManager.Instance.playerStats.Points >= upgradingValue.GetCostForNextUpgrade())
        {
            GameManager.Instance.playerStats.Points -= upgradingValue.GetCostForNextUpgrade();
            upgradingValue.UpgradeValue();
            UpdateValues();
        }
    }

    private void UpdateValues()
    {
        value.text = upgradingValue.GetValue().ToString();
        bool unlocked = (parentPanel == null) ? false : !parentPanel.WeaponUnlocked(); 
        if (!upgradingValue.CanUpgrade() || unlocked)
        {
            costValue.gameObject.SetActive(false);
            costText.gameObject.SetActive(false);
            upgradeButton.gameObject.SetActive(false);
            nextValue.gameObject.SetActive(false);
            nextText.gameObject.SetActive(false);
            return;
        }


        if (upgradingValue.CanUpgrade() && playerStats.Points >= upgradingValue.GetCostForNextUpgrade())
        {
            upgradeButton.GetComponent<Image>().color = Color.white;
        }
        else if (upgradingValue.CanUpgrade() && playerStats.Points < upgradingValue.GetCostForNextUpgrade())
        {
            upgradeButton.GetComponent<Image>().color = Color.grey;
        }

        costValue.text = upgradingValue.GetCostForNextUpgrade().ToString();
        
        nextValue.text = upgradingValue.GetNextUpgradeValue().ToString();
    }
}
