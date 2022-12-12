using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private static bool GameIsPaused = false;
    private static bool GameIsOver = false;
    public GameObject PauseMenuUI;

    private void Start()
    {
        EventManager.OnPlayerDeath += PlayerDeath;
    }

    // Update is called once per frame
    void Update()
    {
     if (Input.GetKeyDown(KeyCode.Escape) && !GameIsOver)
     {
         // Unlocks cursor and pause or un-pause game
         if (!GameIsPaused)
         {   
             Pause();
         }
         else
         {
             Resume();
         }
     }
    }

    public void Resume()
    {
        // makes component inactive and un-freezes the game
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        GameIsPaused = false;
    }

    private void Pause()
    {
        // makes component active and freezes the game
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.Confined;
        GameIsPaused = true;
    }

    public void Quit()
    {
        // loads the scene with index 0, which should be the Main Menu
        GameIsPaused = false;
        SceneManager.LoadScene(0);
    }
    private void PlayerDeath() {
        // check if our player is in Dead state
        // first get the PlayerStats
        PlayerStats playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        if (playerStats != null) {
            if (playerStats.state == PlayerStats.State.Dead) {
                // player is dead, so we need to load the game over scene
                VictoryFailed();
            }
        }

    }
    private void VictoryFailed() {
        GameObject gameOverUI = GameObject.Find("Canvas").transform.Find("GameOverMenu").gameObject;
        GameIsOver = true;
        GameIsPaused = true;
        Debug.Log("You lose :(");
        if (gameOverUI != null) {
            gameOverUI.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
