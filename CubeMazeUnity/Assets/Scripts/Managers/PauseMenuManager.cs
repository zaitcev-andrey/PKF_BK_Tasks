using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenuPanel;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(_pauseMenuPanel.activeSelf)
            {
                ContinueOnClick();
            }
            else
            {
                _pauseMenuPanel.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    public void ContinueOnClick()
    {
        _pauseMenuPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void RestartGameOnClick()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }

    public void GoToMainMenuOnClick()
    {
        SceneManager.LoadScene(0);
    }
}
