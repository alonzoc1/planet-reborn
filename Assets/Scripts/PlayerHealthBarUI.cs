using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarUI : MonoBehaviour
{
    public Image fill;
    public TextMeshProUGUI label;

    private const string LabelBase = "Health: ";
    private int maxHealth = 100;
    public void SetValue(int current, int max)
    {
        maxHealth = max;
        fill.fillAmount = (float)current / max;
        label.text = LabelBase + current +" / " + max;
    }
    // decrease health bar
public void DecreaseHealth(int amount)
    {
        fill.fillAmount -= (float) amount/100;
        label.text = LabelBase + (int)(fill.fillAmount*100) + " / " + maxHealth;
    }
    
}
