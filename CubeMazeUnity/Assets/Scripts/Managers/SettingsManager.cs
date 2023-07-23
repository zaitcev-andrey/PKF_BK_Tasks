using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenuPanel;
    [SerializeField] private GameObject _settingsPanel;

    [SerializeField] private Slider sliderForGeneralSound;
    [SerializeField] private Slider sliderForRollingBallSound;
    [SerializeField] private Slider sliderForDefeatSound;
   
    private RollingBallSound _rollingBallSound;
    private DefeatSound _defeatSound;

    private void Start()
    {      
        _rollingBallSound = FindObjectOfType<RollingBallSound>();
        _defeatSound = FindObjectOfType<DefeatSound>();
    }

    private void Update()
    {
        _rollingBallSound.SetGlobalVolumeCoef(sliderForGeneralSound.value);
        _defeatSound.SetGlobalVolumeCoef(sliderForGeneralSound.value);
        
        _rollingBallSound.SetLocalVolumeCoef(sliderForRollingBallSound.value);
        _defeatSound.SetLocalVolumeCoef(sliderForDefeatSound.value);
    }

    public void BackToPauseMenuPanelOnClick()
    {
        _settingsPanel.SetActive(false);
        _pauseMenuPanel.SetActive(true);
    }
}
