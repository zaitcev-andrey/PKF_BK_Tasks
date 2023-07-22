using System;
using UnityEngine;
using UnityEngine.UI;

public class FinishManager : MonoBehaviour
{
    [SerializeField] private GameObject _informationDuringGamePanel;
    [SerializeField] private GameObject _finishPanel;
    [SerializeField] private Text _timerTextForFinishPanel;

    private Timer _timer;

    void Start()
    {
        _timer = FindObjectOfType<Timer>();
    }

    public void FinishScenario()
    {
        _informationDuringGamePanel.SetActive(false);
        _finishPanel.SetActive(true);
        float time = _timer.GetTime();
        _timerTextForFinishPanel.text = Math.Round(time, 2).ToString();
    }
}
