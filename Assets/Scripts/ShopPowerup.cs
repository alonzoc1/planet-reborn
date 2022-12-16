using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShopPowerup : MonoBehaviour {
    public GameObject powerupCube;
    private Vector3 rotate;
    public int cost;
    public Powerup powerup;
    private CoinCounter coins;
    private bool cubeDead;

    public enum Powerup {
        Speed,
        Damage,
        Health,
        Jump
    };

    private void Start() {
        rotate = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)).normalized;
        coins = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<CoinCounter>();
        cubeDead = false;
    }

    private void Update() {
        if (!cubeDead)
            powerupCube.transform.Rotate(rotate * (Time.deltaTime * 50f));
    }

    public void Touched() {
        if (CurrencyPersist.Instance.GetCoins() >= cost) {
            coins.BuyThing(cost);
            switch (powerup) {
                case Powerup.Damage:
                    PowerupsPersist.Instance.damageBuff = 1.5f;
                    break;
                case Powerup.Health:
                    PowerupsPersist.Instance.healthBuffAdditive = 30;
                    // Update health right away THIS IS HARDCODED SINCE WE'RE RUNNING OUT OF TIME :) - Alonzo
                    GameObject.FindGameObjectWithTag("PlayerHealthBar").GetComponent<PlayerHealthBarUI>().SetValue(130, 130);
                    break;
                case Powerup.Jump:
                    PowerupsPersist.Instance.jumpBuff = 1.5f;
                    break;
                case Powerup.Speed:
                    PowerupsPersist.Instance.speedBuff = 1.5f;
                    break;
            }

            cubeDead = true;
            Destroy(powerupCube);
        }
    }
}
