using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    private float _acceleration = 5.0f;
    [SerializeField]
    private float _moveStepSize = 10.0f;
    [SerializeField]
    private float _moveStepSpeed = 10.0f;

    private InputAction _thorottleAction;
    private InputAction _moveLeftAction;
    private InputAction _moveRightAction;
    private InputAction _parryAction;

    private Vector3 _nextPosition;
    private float _speed;
    private float _gasPoint;
    private int _healthPoint;

    private void OnEnable()
    {
        _thorottleAction = playerInput.actions["Accelerate"];
        _moveLeftAction = playerInput.actions["MoveLeft"];
        _moveRightAction = playerInput.actions["MoveRight"];
        _parryAction = playerInput.actions["Parry"];

        _moveLeftAction.performed += OnMoveLeft;
        _moveRightAction.performed += OnMoveRight;
        _parryAction.performed += OnParry;
    }

    private void Update()
    {
        Accelerate();
        transform.position = Vector3.Lerp(transform.position, _nextPosition, Time.deltaTime * _moveStepSpeed);

    }

    private void Accelerate()
    {
        float verticalInput = _thorottleAction.ReadValue<float>();
        if (verticalInput < 0f)
        {
            _speed += _acceleration * Time.deltaTime;
        }

        else if (verticalInput < 0f)
        {
            _speed -= _acceleration * Time.deltaTime;
        }
    }

    private void OnParry(InputAction.CallbackContext context)
    {

    }

    private void OnMoveLeft(InputAction.CallbackContext context)
    {
        _nextPosition = transform.position + (-transform.right * _moveStepSize);
    }

    private void OnMoveRight(InputAction.CallbackContext context)
    {
        _nextPosition = transform.position + (transform.right * _moveStepSize);

    }
}