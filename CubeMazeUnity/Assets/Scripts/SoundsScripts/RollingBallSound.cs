using UnityEngine;

public class RollingBallSound : MonoBehaviour
{
    [SerializeField] private AudioSource _RollingBallSound;
    [SerializeField] private Rigidbody _playerRigidbody;

    private float _localVolumeCoef = 1f;
    private float _globalVolumeCoef = 1f;

    private void Update()
    {
        if (_playerRigidbody.velocity.magnitude / 10 < 0.1)
            _RollingBallSound.volume = 0f;
        else
            _RollingBallSound.volume = _playerRigidbody.velocity.magnitude / 10 * _localVolumeCoef * _globalVolumeCoef;
    }

    public void StopSound()
    { 
        _RollingBallSound.Stop();
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
