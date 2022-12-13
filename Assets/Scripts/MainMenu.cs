using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour {
    public TMP_Dropdown leftDropdown;
    public TMP_Dropdown rightDropdown;
    
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
}