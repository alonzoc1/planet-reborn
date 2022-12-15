using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetGame : MonoBehaviour
{
    private void Start() {
        Cursor.lockState = CursorLockMode.Confined; // Unlock cursor
    }

    // Need this script to correctly reset game and undo persistence
    public void ResetToMainMenu() {
        if (LoadoutPersist.Instance != null)
            Destroy(LoadoutPersist.Instance.gameObject);
        if (CurrencyPersist.Instance != null)
            Destroy(CurrencyPersist.Instance.gameObject);
        SceneManager.LoadScene(0);
    }
}
