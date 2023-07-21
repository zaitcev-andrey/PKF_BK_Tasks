using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerForCubeMazeBackRotation : MonoBehaviour
{
    [SerializeField] private Transform _cubeMazeWithPlayer;
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_gameManager.BackTriggers[0].activeSelf)
        {
            ChangeTriggers(0);
            DoRotation(0);
        }
        else if (_gameManager.BackTriggers[1].activeSelf)
        {
            ChangeTriggers(1);
            DoRotation(1);
        }
        else if (_gameManager.BackTriggers[2].activeSelf)
        {
            ChangeTriggers(2);
            DoRotation(2);
        }
        else if (_gameManager.BackTriggers[3].activeSelf)
        {
            ChangeTriggers(3);
            DoRotation(3);

        }
        else if (_gameManager.BackTriggers[4].activeSelf)
        {
            ChangeTriggers(4);
            DoRotation(4);
        }
    }

    private void ChangeTriggers(int index)
    {
        _gameManager.BackTriggers[index].SetActive(false);
        if(index != 4)
            _gameManager.ForwardTriggers[index + 1].SetActive(false);

        _gameManager.ForwardTriggers[index].SetActive(true);
        if (index != 0)
            _gameManager.BackTriggers[index - 1].SetActive(true);
    }

    private void DoRotation(int index)
    {
        _cubeMazeWithPlayer.eulerAngles = AllValuesOfRotationAndPositionForCubeMaze.GetRotation(index);
        _cubeMazeWithPlayer.position = AllValuesOfRotationAndPositionForCubeMaze.GetPosition(index);
    }
}
