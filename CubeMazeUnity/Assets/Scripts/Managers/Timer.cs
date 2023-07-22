using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float _time;
    [SerializeField] private Text _timerText;
    private void Start()
    {
        _time = 0f;
    }

    private void Update()
    {
        _time += Time.deltaTime;
        _timerText.text = Mathf.Round(_time).ToString();
    }

    public void DefaultSettings()
    {
        _time = 0f;
    }

    public float GetTime()
    {
        return _time;
    }
}
