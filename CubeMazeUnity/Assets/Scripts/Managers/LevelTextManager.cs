using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTextManager : MonoBehaviour
{
    [SerializeField] private Text[] _allLevels;

    public void ChangeColorForLevelText(int index)
    {
        _allLevels[index].color = Color.green;
        _allLevels[index + 1].color = Color.white;
    }

    public void DefaultSettings()
    {
        foreach (var item in _allLevels)
        {
            Color color = new Color(180f / 255, 180f / 255, 180f / 255);
            item.color = color;
        }
        _allLevels[0].color = Color.white;
    }
}
