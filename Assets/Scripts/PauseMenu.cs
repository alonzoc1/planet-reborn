using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private static bool GameIsPaused = false;

    public GameObject PauseMenuUI;
    // Update is called once per frame
    void Update()
    {
     if (Input.GetKeyDown(KeyCode.Escape))
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
}
