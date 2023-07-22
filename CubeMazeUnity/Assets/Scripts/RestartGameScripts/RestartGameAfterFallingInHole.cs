using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class RestartGameAfterFallingInHole : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private ParticleSystem _burstEffect;
    private PauseMenuManager _pauseMenuManager;

    private void Start()
    {
        _pauseMenuManager = FindObjectOfType<PauseMenuManager>();
    }

    public void RestartScenarioAfterFallingInHole()
    {
        _burstEffect.transform.position = _player.transform.position;
        _burstEffect.Play();
        StartCoroutine(RespawnProcess());
    }

    private IEnumerator RespawnProcess()
    {
        _player.SetActive(false);
        yield return new WaitForSecondsRealtime(2);
        _pauseMenuManager.RestartGameOnClick();
    }
}
