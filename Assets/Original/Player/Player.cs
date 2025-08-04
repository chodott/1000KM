using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region SerializeField
    [SerializeField]
    private PlayerInput _playerInput;
    [SerializeField]
    private BoxCollider _boxCollider;
    [SerializeField]
    private float _acceleration = 5.0f;
    [SerializeField]
    private float _moveStepSize = 10.0f;
    [SerializeField]
    private float _moveStepSpeed = 10.0f;
    [SerializeField]
    private float _parryCooldownTime = 0.1f;

    #endregion

    private InputAction _thorottleAction;
    private InputAction _moveLeftAction;
    private InputAction _moveRightAction;
    private InputAction _parryAction;


    private Vector3 _nextPosition;
    private float _speed;
    private float _parryCooldownTimer;
    private float _gasPoint;
    private int _healthPoint;
    private bool _canParry = true;

    private void OnEnable()
    {
        _thorottleAction = _playerInput.actions["Accelerate"];
        _moveLeftAction = _playerInput.actions["MoveLeft"];
        _moveRightAction = _playerInput.actions["MoveRight"];
        _parryAction = _playerInput.actions["Parry"];

        _moveLeftAction.performed += OnMoveLeft;
        _moveRightAction.performed += OnMoveRight;
        _parryAction.performed += OnParry;
    }

    private void Update()
    {
        Accelerate();
        transform.position = Vector3.Lerp(transform.position, _nextPosition, Time.deltaTime * _moveStepSpeed);

        if(_canParry == false)
        {
            _parryCooldownTimer += Time.deltaTime;
            if(_parryCooldownTimer > _parryCooldownTime)
            {
                _parryCooldownTimer = 0.0f;
                _canParry = true;
            }
        }

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

    #region Collider Callbacks
    public void OnTriggerStay(Collider other)
    {
        Debug.Log("Parry Success");
        if (other.TryGetComponent<IParryable>(out var parryable))
        {
            parryable.OnParried();
        }
    }
    #endregion

    #region PlayerInput Callbacks
    private void OnParry(InputAction.CallbackContext context)
    {
        Debug.Log("Press SpaceBar");
        if (_canParry == true)
        {
            _boxCollider.enabled = true;
            _canParry = false;
        }
    }

    private void OnMoveLeft(InputAction.CallbackContext context)
    {
        _nextPosition = transform.position + (-transform.right * _moveStepSize);
    }

    private void OnMoveRight(InputAction.CallbackContext context)
    {
        _nextPosition = transform.position + (transform.right * _moveStepSize);

    }
    #endregion
}