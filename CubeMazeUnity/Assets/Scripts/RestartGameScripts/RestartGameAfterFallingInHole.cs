using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class RestartGameAfterFallingInHole : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private ParticleSystem _burstEffect;
    [SerializeField] private RestartGame _restartGame;

    public void RestartScenarioAfterFallingInHole()
    {
        _burstEffect.transform.position = _player.transform.position;
        _burstEffect.Play();
        StartCoroutine(SetDefaultRotationAndPositionOfCubeMaze());
    }

    private IEnumerator SetDefaultRotationAndPositionOfCubeMaze()
    {
        _player.SetActive(false);
        yield return new WaitForSecondsRealtime(2);
        _restartGame.RestartOnClick();
    }
}
