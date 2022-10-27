using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using TMPro;

public class CoinCounter : MonoBehaviour
{
	public TMP_Text coinCounter;
	public GameObject player;
	private void Update()
	{
		coinCounter.text = player.GetComponent<PlayerInventory>().currentCoins.ToString();
	}
}
