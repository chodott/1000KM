using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IDamagable
{
    #region SerializeField
    [SerializeField]
    private PlayerInput _playerInput;
    [SerializeField]
    private PartManager _partManager;
    [SerializeField]
    private PlayerMovement _playerMovement;
    [SerializeField]
    private PlayerParrySystem _playerParrySystem;
    [SerializeField]
    private PlayerStatus _playerStatus;
    [SerializeField]
    private LaneMover _laneMover;
    [SerializeField]
    private InvincibleSystem _invincibility;
    [SerializeField]
    private HitCollider _hitCollider;
    [SerializeField]
    private StunParticleHandler _stunParticleHandler;
    #endregion

    private IPlayerState _curState;

    private PlayerDriveState _driveState = new PlayerDriveState();
    private PlayerParryState _parryState = new PlayerParryState();
    private NoInputState _noInputState = new NoInputState();
    private PlayerStunState _stunState = new PlayerStunState();

    private InputAction _thorottleAction;
    private InputAction _moveLeftAction;
    private InputAction _moveRightAction;
    private InputAction _parryAction;

    public PlayerStatus Status { get {return _playerStatus; } }
    public PlayerParrySystem ParrySystem { get { return _playerParrySystem; } }
    public PlayerMovement PlayerMovement { get { return _playerMovement; } }    
    public LaneMover LaneMover { get { return _laneMover; } }

    #region Monobehavour Callbacks

    private void Start()
    {
        ChangeState(_driveState);
    }
    private void OnEnable()
    {
        _thorottleAction = _playerInput.actions["Accelerate"];
        _moveLeftAction = _playerInput.actions["MoveLeft"];
        _moveRightAction = _playerInput.actions["MoveRight"];
        _parryAction = _playerInput.actions["Parry"];

        _moveLeftAction.performed += OnMoveLeft;
        _moveRightAction.performed += OnMoveRight;
        _parryAction.performed += OnParry;
        _partManager.OnChangedPartStatus += UpdateStatus;

        _invincibility.OnFinishedInvincible += StopStunEffect;

        GameEvents.OnPhaseChanged += OnPhaseChanged;

    }



    private void OnDisable()
    {
        _moveLeftAction.performed -= OnMoveLeft;
        _moveRightAction.performed -= OnMoveRight;
        _parryAction.performed -= OnParry;
        _partManager.OnChangedPartStatus -= UpdateStatus;
    }

    private void Update()
    {
        _playerMovement.SetMoveDirection(_thorottleAction.ReadValue<float>());
    }

    private void UpdateStatus(PartStatus status)
    {
        _playerMovement.UpdateStatus(status);
        _playerStatus.UpdateStatus(status);
        _laneMover.UpdateMoveLaneSpeed(status.LaneMoveSpeedBonus);

        _playerParrySystem.Init(_laneMover.MoveLaneSpeed);
    }

    private void OnPhaseChanged(GamePhase phase, PhaseData data)
    {
        switch(phase)
        {
            case GamePhase.Shop:
                EnterDriveState();
                break;
            case GamePhase.Normal:
                _playerMovement.UnlockAcceleration();
                break;
            case GamePhase.BossIntro:
                _playerMovement.LockAcceleration(data.lockVelocity, data.duration);
                break;
        }
    }

    #endregion

    #region PlayerInput Callbacks
    private void OnParry(InputAction.CallbackContext context)
    {
        _curState.OnParry(context);
    }

    private void OnMoveLeft(InputAction.CallbackContext context)
    {
        _curState.OnMoveLeft(context);
    }

    private void OnMoveRight(InputAction.CallbackContext context)
    {
        _curState.OnMoveRight(context);
    }
    #endregion

    #region State Switching
    private void ChangeState(IPlayerState state)
    {
        if(_curState != null)
        {
            _curState.Exit();
        }
        _curState = state;
        _curState.Enter(this);
    }
    public void EnterDriveState()
    {
        ChangeState(_driveState);
    }
    public void EnterParryState()
    {
        ChangeState(_parryState);
    }
    public void EnterNoInputState()
    {
        ChangeState(_noInputState);
    }

    public void EnterStunState()
    {
        ChangeState(_stunState);
    }
    #endregion;


    private void StopStunEffect()
    {
        _stunParticleHandler.StopStunEffect();
    }

    public void StartStun()
    {
        _invincibility.StartInvinble();
        _stunParticleHandler.PlayStunEffect();
    }

    public bool OnDamaged(float amount)
    {
        if (_invincibility.IsActive)
        {
            return true;
        }

        if (_hitCollider.IsActive == false)
        {
            return false;
        }
        _playerStatus.OnDamaged(amount);

        _curState.OnDamaged();
        return true;
    }
}