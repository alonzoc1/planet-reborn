using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject PauseMenuUI;
    // Update is called once per frame
    void Update()
    {
     if (Input.GetKeyDown(KeyCode.Escape))
     {
         // Unlocks cursor and pause or un-pause game
         if (!GameIsPaused)
         {   
             Cursor.lockState = CursorLockMode.Confined;
             Pause();
         }
         else
         {
             Cursor.lockState = CursorLockMode.Locked;
             Resume();
         }
     }
    }

    public void Resume()
    {
        // makes component inactive and un-freezes the game
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    private void Pause()
    {
        // makes component active and freezes the game
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Quit()
    {
        // Closes the game, doesn't work in editor.
        Debug.Log("Game Quit!");
        Application.Quit();
    }
}
