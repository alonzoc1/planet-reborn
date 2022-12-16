using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyPersist : MonoBehaviour {
    public static CurrencyPersist Instance;

    private int coins;
    
    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        coins = 0; // Start at 0
    }

    public int GetCoins() {
        return coins;
    }

    public int IncrementCoins() {
        return ++coins; // increment, then returns new value
    }

    public int PayCoins(int amount) {
        coins -= amount;
        return coins;
    }

    public void SetCoins(int value) {
        coins = value;
    }
}
