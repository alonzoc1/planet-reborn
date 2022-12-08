using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private int currentCoins;
    private CoinCounter coinCounter;
    private PlayerHealthBarUI healthBar;

    private void Start() {
        coinCounter = GameObject.FindWithTag("MainCanvas").GetComponent<CoinCounter>();
        healthBar = GameObject.FindWithTag("PlayerHealthBar").GetComponent<PlayerHealthBarUI>();
        currentCoins = 0; // Get this from a persistant source in the future
        coinCounter.UpdateCount(currentCoins);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            currentCoins += 1;
            coinCounter.UpdateCount(currentCoins);
            Destroy(other.gameObject);
        }
    }
}