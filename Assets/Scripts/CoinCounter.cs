using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

// handles UI work for coin counter
public class CoinCounter : MonoBehaviour
{
	public TextMeshProUGUI coinCounter;
	private const string LabelBase = "Coins: ";
	private CurrencyPersist currencyPersist;

	private void Start() {
		currencyPersist = CurrencyPersist.Instance;
		SetCountUI(currencyPersist.GetCoins());
	}

	public void UpdateCount(int value) {
		currencyPersist.SetCoins(value);
		SetCountUI(value);
	}

	public void IncrementCoins() {
		SetCountUI(currencyPersist.IncrementCoins());
	}

	private void SetCountUI(int value) {
		coinCounter.text = LabelBase + value;
	}
}
