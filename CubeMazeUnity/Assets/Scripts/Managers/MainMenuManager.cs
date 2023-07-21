using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartGameOnClick()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1.0f;
    }

    public void ExitGameOnClick()
    {
        Application.Quit();
    }
}
