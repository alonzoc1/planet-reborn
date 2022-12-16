using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour {
    public VictoryConditions victory;
    public GameObject glow;

    private Material glowy;
    private Color glowColor;

    private const float MaxGlow = 1f;
    
    public void Activate() {
        victory.TeleporterActivated();
        glowy = glow.GetComponent<Renderer>().material;
        glowColor = glowy.color;
    }

    public void SetGlowProgress(float progress) {
        glowy.SetColor("_EmissionColor", glowColor * progress * MaxGlow);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
            victory.SetPlayerInTeleporterRange(true);
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player"))
            victory.SetPlayerInTeleporterRange(false);
    }
}
