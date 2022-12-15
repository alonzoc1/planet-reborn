using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private CoinCounter coinCounter;
    private PlayerHealthBarUI healthBar;
    private CurrencyPersist currencyPersist; // stores our coins, only exists if starting from Start Menu

    private void Start() {
        currencyPersist = CurrencyPersist.Instance;
        coinCounter = GameObject.FindWithTag("MainCanvas").GetComponent<CoinCounter>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            coinCounter.IncrementCoins();
            Destroy(other.gameObject);
        }
    }
}