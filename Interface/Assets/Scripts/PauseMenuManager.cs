using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenuPanel;
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (_pauseMenuPanel.activeSelf)
            {
                ContinueOnCLick();
            }
            else
            {
                Time.timeScale = 0f;
                _pauseMenuPanel.SetActive(true);
            }
        }
    }

    public void ContinueOnCLick()
    {
        Time.timeScale = 1f;
        _pauseMenuPanel.SetActive(false);
    }

    public void ExitOnCLick()
    {
        Application.Quit();
    }
}
