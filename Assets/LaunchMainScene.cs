using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaunchMainScene : MonoBehaviour
{
    void Start()
    {
        Invoke("LoadNextScene", 10f); //Invoke pour �tre "m�ta"
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene("Game");
    }

}
