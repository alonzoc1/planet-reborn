using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarUI : MonoBehaviour
{
    public Image fill;
    public TextMeshProUGUI label;
    private int maxHealth = 100;
    // make current health a property so that we can set it and it will update the UI
    public int currentHealth {get; private set;}
    private const string LabelBase = "Health: ";
    public void SetValue(int current, int max)
    {
        maxHealth = max;
        currentHealth = current;
        fill.fillAmount = (float)current / max;
        label.text = LabelBase + current +" / " + max;
    }
    public void DecreaseHealth(int amount)
    {
        SetValue(currentHealth - amount, maxHealth);
    }
    
}
