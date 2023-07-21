using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartGame : MonoBehaviour
{
    [SerializeField] private Transform _cubeMazeWithPlayer;
    [SerializeField] private Transform _startPositionForPlayer;
    [SerializeField] private GameObject _player;

    public void RestartOnClick()
    {
        _cubeMazeWithPlayer.eulerAngles = AllValuesOfRotationAndPositionForCubeMaze.GetRotation(0);
        _cubeMazeWithPlayer.position = AllValuesOfRotationAndPositionForCubeMaze.GetPosition(0);
        _player.transform.position = _startPositionForPlayer.position;
        _player.SetActive(true);
    }
}
