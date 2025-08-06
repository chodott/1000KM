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
    private PlayerMovement _playerMovement;
    [SerializeField]
    private PlayerParrySystem _playerParrySystem;
    [SerializeField]
    private PlayerStatus _playerStatus;
    [SerializeField]
    private LaneMover _laneMover;
    #endregion

    private InputAction _thorottleAction;
    private InputAction _moveLeftAction;
    private InputAction _moveRightAction;
    private InputAction _parryAction;

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
        _playerMovement.SetMoveDirection(_thorottleAction.ReadValue<float>());
    }

    #region PlayerInput Callbacks
    private void OnParry(InputAction.CallbackContext context)
    {
        _playerParrySystem.Parry();
    }

    private void OnMoveLeft(InputAction.CallbackContext context)
    {
        _laneMover.MoveLane(-1);
    }

    private void OnMoveRight(InputAction.CallbackContext context)
    {
        _laneMover.MoveLane(1);
    }
    #endregion
}