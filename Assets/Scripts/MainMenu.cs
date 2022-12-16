using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    public TMP_Dropdown leftDropdown;
    public TMP_Dropdown rightDropdown;
    public Slider volumeSlider;
    
    public void PlayGame()
    {
        LoadoutPersist.Instance.StoreAbilities(leftDropdown.options[leftDropdown.value].text, rightDropdown.options[rightDropdown.value].text);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void SetVolume() {
        OptionsPersist.Instance.volume = volumeSlider.value;
        Camera.main.gameObject.GetComponent<AudioSource>().volume = OptionsPersist.Instance.volume;
    }

    public void SetSlider() {
        volumeSlider.value = OptionsPersist.Instance.volume;
    }

    private void Start() {
        // Reset persistents if they exist
        if (CurrencyPersist.Instance != null)
            CurrencyPersist.Instance.SetCoins(0);
        if (PowerupsPersist.Instance != null)
            PowerupsPersist.Instance.Reset();
    }
}