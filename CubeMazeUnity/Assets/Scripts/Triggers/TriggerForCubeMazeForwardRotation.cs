using UnityEngine;

public class TriggerForCubeMazeForwardRotation : MonoBehaviour
{
    [SerializeField] private Transform _cubeMazeWithPlayer;
    private GameManager _gameManager;
    private LevelTextManager _levelsTextManager;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _levelsTextManager = FindObjectOfType<LevelTextManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_gameManager.ForwardTriggers[0].activeSelf)
        {
            SetChanges(0);
        }
        else if (_gameManager.ForwardTriggers[1].activeSelf)
        {
            SetChanges(1);
        }
        else if (_gameManager.ForwardTriggers[2].activeSelf)
        {
            SetChanges(2);
        }
        else if (_gameManager.ForwardTriggers[3].activeSelf)
        {
            SetChanges(3);
        }
        else if (_gameManager.ForwardTriggers[4].activeSelf)
        {
            SetChanges(4);
        }
    }

    private void SetChanges(int index)
    {
        ChangeTriggers(index);
        DoRotation(index);
        _levelsTextManager.ChangeColorForLevelText(index);
    }

    private void ChangeTriggers(int index)
    {
        _gameManager.ForwardTriggers[index].SetActive(false);
        if(index != 0)
            _gameManager.BackTriggers[index - 1].SetActive(false);

        _gameManager.BackTriggers[index].SetActive(true);
        if(index != 4)
            _gameManager.ForwardTriggers[index + 1].SetActive(true);
    }

    private void DoRotation(int index)
    {
        _cubeMazeWithPlayer.eulerAngles = AllValuesOfRotationAndPositionForCubeMaze.GetRotation(index + 1);
        _cubeMazeWithPlayer.position = AllValuesOfRotationAndPositionForCubeMaze.GetPosition(index + 1);
    }
}
