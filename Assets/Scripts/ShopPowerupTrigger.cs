using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPowerupTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
            gameObject.GetComponentInParent<ShopPowerup>().Touched();
    }
}
