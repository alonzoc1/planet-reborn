using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsPersist : MonoBehaviour {
    public static OptionsPersist Instance;
    public float volume = .5f;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
