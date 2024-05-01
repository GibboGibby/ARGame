using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class ScriptableObjectIdAttribute : PropertyAttribute { }


#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ScriptableObjectIdAttribute))]
public class ScriptableObjectIdDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;
        if (property.stringValue == "Single") property.stringValue = "float";
        if (property.stringValue == "Int32") property.stringValue = "int";  
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}
#endif

 
public abstract class UpgradeableValueScriptableObject<T> : ScriptableObject
{
    
    [ScriptableObjectId]
    public string type = typeof(T).Name;
    


    [System.Serializable]
    public struct Upgrade
    {
        [SerializeField] public T value;
        [SerializeField] public int cost;
    }

    public T defaultValue;

    // These are in the form of the final    values not how they change
    [SerializeField] public List<Upgrade> upgrades = new List<Upgrade>();

    private int currentUpgrade = -1;


    /// <summary>
    /// Resets the progress of the upgrade
    /// </summary>
    public void ResetProgress()
    {
        currentUpgrade = -1;
    }

    public int CurrentUpgradeNumber()
    {
        return currentUpgrade;
    }

    /// <summary>
    /// Gets the current value
    /// </summary>
    /// <returns>The current value that is being stored</returns>
    public T GetValue()
    {
        if (currentUpgrade == -1)
        {
            return defaultValue;
        }
        else
        {
            return upgrades[currentUpgrade].value;
        }
    }

    /// <summary>
    /// Returns the cost for the next upgrade
    /// </summary>
    /// <returns>-1 means that the object is at max upgrade<br/>Other values indicate the cost of the upgrade</returns>
    public int GetCostForNextUpgrade()
    {
        if (IsFullyUpgraded()) { return -1; }
        return upgrades[currentUpgrade + 1].cost;
    }

    public string GetAllUpgradesAndCosts()
    {
        string returnStr = $"Default value - {defaultValue}\nNumber of upgrades - {upgrades.Count}\n";
        if (upgrades.Count == 0) { return returnStr; }
        for (int i = 0; i < upgrades.Count; i++)
        {
            returnStr += $"{i+1}th upgrade: value - {upgrades[i].value} and cost - {upgrades[i].cost}\n";
        }
        return returnStr;
    }

    public void UpgradeValue()
    {
        currentUpgrade++;
        if (currentUpgrade >= upgrades.Count-1)
        {
            currentUpgrade = upgrades.Count-1;
        }
    }

    public bool IsFullyUpgraded()
    {
        if (upgrades.Count - 1 == currentUpgrade)
            return true;
        return false;
    }
}