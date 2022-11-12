using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewScene : MonoBehaviour
{

    // Allows scene to change once the object this script is attached to is touched.
    public void OnTriggerEnter()
    {
        ChangeScene();
    }

    //Swap to next Scene
    public void ChangeScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

}

