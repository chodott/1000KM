using System.Reflection;
using UnityEngine.InputSystem;

public class PlayerDriveState : IState<Player>
{
    private Player _player;
    private LaneMover _laneMover;
    private PlayerMovement _playerMovement;
    private PlayerStatus _playerStatus;
    public void Enter(Player player)
    {
        _player = player;
        _laneMover = player.LaneMover;
        _playerMovement = player.PlayerMovement;
        _playerStatus = player.Status;
    }

    public void Exit()
    {
        _player = null;
        _laneMover = null;
        _playerMovement = null;
        _playerStatus = null;
    }

    public void HandleEvent(StateEvent eventType)
    {
        switch (eventType)
        {
            case OnDamagedEvent:
                _playerMovement.OnDamaged();
                _player.EnterStunState();
                break;

            case InputMoveEvent inputEvent:
                _laneMover.MoveLane(inputEvent.IsRight, false);
                break;

            case InputParryEvent:
                _player.EnterParryState();
                break;


            default:
                break;
        }
    }

    public void Update()
    {
    }
}

public class PlayerParryState : IState<Player>
{
    private Player _player;
    private PlayerParrySystem _playerParrySystem;
    private LaneMover _laneMover;
    public void Enter(Player player)
    {
        _player = player;
        _laneMover = player.LaneMover;
        _laneMover.StopAnimation();
        _playerParrySystem = player.ParrySystem;
        _playerParrySystem.Parry();
        _playerParrySystem.OnParryFinished += EndParry;
    }

    public void Exit()
    {
        _playerParrySystem.OnParryFinished -= EndParry;
        _laneMover = null;
        _playerParrySystem = null;
        _player = null;
    }

    public void HandleEvent(StateEvent eventType)
    {
        switch (eventType)
        {
            case InputMoveEvent moveEvent:
                _laneMover.MoveLane(moveEvent.IsRight, false, true);
                break;
            default:
                break;
        }
    }

    public void Update()
    {

    }
    private void EndParry()
    {
        _player.EnterDriveState();
    }
}

public class PlayerStunState : IState<Player>
{
    private Player _player;
    private InvincibleSystem _invincibleSystem;
    private PlayerMovement _playerMovement;
    public void Enter(Player player)
    {
        _player = player;
        _playerMovement = _player.PlayerMovement;
        _invincibleSystem = _player.GetComponent<InvincibleSystem>();

        _player.StartStun();
        _invincibleSystem.OnFinishedInvincible += EndStun;
        _playerMovement.LockAcceleration();
    }

    public void Exit()
    {
        _invincibleSystem.OnFinishedInvincible -= EndStun;
        _playerMovement.UnlockAcceleration();
        _playerMovement = null;
        _invincibleSystem = null;
        _player = null;
    }

    public void HandleEvent(StateEvent eventType)
    {
    }

    public void Update()
    {
        return;
    }

    private void EndStun()
    {
        _player.EnterDriveState();
    }
}

public class NoInputState : IState<Player>
{
    public void Enter(Player player)
    {
        GameEvents.SetPhase(GamePhase.Shop, new PhaseData());
    }

    public void Exit()
    {

    }

    public void HandleEvent(StateEvent eventType)
    {
    }
    public void Update()
    {
        return;
    }
}