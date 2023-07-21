using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(5, 20)] private float _speed;

    private Rigidbody _playerRigidbody;
    private float _x, _z;
    private Vector3 _movement;

    private void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void SetMovementVector()
    {
        _x = Input.GetAxis(GlobalStringVars.HORIZONTAL_AXIS);
        _z = Input.GetAxis(GlobalStringVars.VERTICAL_AXIS);

        _movement = new Vector3(_x, 0, _z).normalized * _speed;
    }
    private void Movement()
    {
        SetMovementVector();
        _playerRigidbody.AddForce(_movement);
    }
}
