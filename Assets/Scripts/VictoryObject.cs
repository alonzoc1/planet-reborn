using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        Debug.Log("Hit victory object");
        if (other.gameObject.CompareTag("Player"))
            EventManager.ReportVictoryObjectTouched();
    }
}
