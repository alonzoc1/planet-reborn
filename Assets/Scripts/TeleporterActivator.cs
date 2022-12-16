using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterActivator : MonoBehaviour {
    public Teleporter teleporter;

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.CompareTag("Player") && Input.GetKey(KeyCode.E))
            teleporter.Activate();
    }
}
