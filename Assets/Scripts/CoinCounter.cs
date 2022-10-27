using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;


public class CoinCounter : MonoBehaviour
{
	public TextMeshProUGUI coinCounter;
	private const string LabelBase = "Coins: ";
	

	public void UpdateCount(int value)
	{
		coinCounter.text = LabelBase + value;
	}
}
