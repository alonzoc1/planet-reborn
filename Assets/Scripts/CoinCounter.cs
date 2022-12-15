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
		if (currencyPersist == null)
			Debug.Log("CurrencyPersist not found, probably because we didn't start from the start menu :)");
		else
			SetCountUI(currencyPersist.GetCoins());
	}

	public void UpdateCount(int value) {
		if (currencyPersist != null)
			currencyPersist.SetCoins(value);
		SetCountUI(value);
	}

	public void IncrementCoins() {
		if (currencyPersist != null)
			SetCountUI(currencyPersist.IncrementCoins());
		else {
			Debug.Log("CurrencyPersist not found, not incrementing coin...");
		}
	}

	private void SetCountUI(int value) {
		coinCounter.text = LabelBase + value;
	}
}
