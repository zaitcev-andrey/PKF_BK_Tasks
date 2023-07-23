using UnityEngine;

public class DefeatSound : MonoBehaviour
{
    [SerializeField] private AudioSource _defeatSound;

    private float _localVolumeCoef = 1f;
    private float _globalVolumeCoef = 1f;

    public void PlayDefeatSound()
    {
        _defeatSound.volume *= _localVolumeCoef * _globalVolumeCoef;
        _defeatSound.Play();
    }

    public void SetLocalVolumeCoef(float value)
    {
        _localVolumeCoef = value;
    }

    public void SetGlobalVolumeCoef(float value)
    {
        _globalVolumeCoef = value;
    }
}
