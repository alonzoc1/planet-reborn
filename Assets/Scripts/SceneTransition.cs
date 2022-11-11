using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public int SceneIndex;
    void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(SceneIndex); // We should make this dynamically change scenes later. For the sake of the demo, I won't do much about it.
    }
}
