using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaunchMainScene : MonoBehaviour
{
    void Start()
    {
        Invoke("LoadNextScene", 10f); //Invoke pour être "méta"
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene("Game");
    }

}
