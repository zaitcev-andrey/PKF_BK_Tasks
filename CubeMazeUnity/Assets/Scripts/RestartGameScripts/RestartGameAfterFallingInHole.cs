using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class RestartGameAfterFallingInHole : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private ParticleSystem _burstEffect;
    private PauseMenuManager _pauseMenuManager;
    private DefeatSound _defeatSound;
    private RollingBallSound _rollingBallSound;

    private void Start()
    {
        _pauseMenuManager = FindObjectOfType<PauseMenuManager>();
        _defeatSound = FindObjectOfType<DefeatSound>();
        _rollingBallSound = FindObjectOfType<RollingBallSound>();
    }

    public void RestartScenarioAfterFallingInHole()
    {
        _burstEffect.transform.position = _player.transform.position;
        _burstEffect.Play();
        _rollingBallSound.StopSound();
        _defeatSound.PlayDefeatSound();
        StartCoroutine(RespawnProcess());
    }

    private IEnumerator RespawnProcess()
    {
        _player.SetActive(false);
        yield return new WaitForSecondsRealtime(2);
        _pauseMenuManager.RestartGameOnClick();
    }
}
