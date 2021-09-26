using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameScene : MonoBehaviour
{
    public void LoadMain()
    {
        Debug.Log("Loading scene");
        SceneManager.LoadScene("Main");
    }

    public void LoadMenu()
    {
        Debug.Log("Loading scene");
        SceneManager.LoadScene("MainMenu");
    }
}
