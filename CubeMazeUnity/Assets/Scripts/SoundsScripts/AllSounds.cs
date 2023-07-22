using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllSounds : MonoBehaviour
{
    [SerializeField] private AudioSource[] _allSoundsOnScene;

    public void PauseAllSounds()
    {
        foreach (var sound in _allSoundsOnScene)
        {
            sound.Pause();
        }
    }

    public void UnPauseAllSounds()
    {
        foreach (var sound in _allSoundsOnScene)
        {
            sound.UnPause();
        }
    }
}
