using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenuPanel;
    [SerializeField] private GameObject _settingsPanel;
    private AllSounds _allSounds;

    private void Start()
    {
        Cursor.visible = false;
        _allSounds = FindObjectOfType<AllSounds>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(_pauseMenuPanel.activeSelf || _settingsPanel.activeSelf)
            {
                ContinueOnClick();
            }
            else
            {
                _pauseMenuPanel.SetActive(true);
                Time.timeScale = 0;
                Cursor.visible = true;
                _allSounds.PauseAllSounds();
            }
        }
    }

    public void ContinueOnClick()
    {
        _pauseMenuPanel.SetActive(false);
        _settingsPanel.SetActive(false);
        Cursor.visible = false;
        Time.timeScale = 1;
        _allSounds.UnPauseAllSounds();
    }

    public void SettingsOnClick()
    {
        _pauseMenuPanel.SetActive(false);
        _settingsPanel.SetActive(true);
    }

    public void RestartGameOnClick()
    {
        SceneManager.LoadScene(1);
        Cursor.visible = false;
        Time.timeScale = 1;
    }

    public void GoToMainMenuOnClick()
    {
        SceneManager.LoadScene(0);
    }
}
