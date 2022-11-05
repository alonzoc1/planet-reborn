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
    public void SetValue(int current, int max)
    {
        fill.fillAmount = (float)current / max;
        label.text = LabelBase + current +" / " + max;
    }
    
}
