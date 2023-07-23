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
            SetChanges(0);
        }
        else if (_gameManager.BackTriggers[1].activeSelf)
        {
            SetChanges(1);
        }
        else if (_gameManager.BackTriggers[2].activeSelf)
        {
            SetChanges(2);
        }
        else if (_gameManager.BackTriggers[3].activeSelf)
        {
            SetChanges(3);

        }
        else if (_gameManager.BackTriggers[4].activeSelf)
        {
            SetChanges(4);
        }
    }

    private void SetChanges(int index)
    {
        ChangeTriggers(index);
        DoRotation(index);
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
