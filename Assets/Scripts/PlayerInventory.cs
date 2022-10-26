using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int currentCoins;

    private void Start()
    {
        currentCoins = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("name: " + other.tag);
        if (other.tag == "Coin")
        {
            currentCoins += 1;
            Destroy(other.gameObject);
        }
    }
}