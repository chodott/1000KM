using UnityEngine.InputSystem;

public interface IPlayerState
{
    public void Enter(Player player);

    public void Exit();
    public void OnDamaged();

    public void Update();

    #region PlayerInput Callbacks
    public void OnParry(InputAction.CallbackContext context);
    public void OnMoveLeft(InputAction.CallbackContext context);
    public void OnMoveRight(InputAction.CallbackContext context);
    #endregion
}



public class PlayerDriveState : IPlayerState
{
    private Player _player;
    private LaneMover _laneMover;
    public void Enter(Player player)
    {
        _player = player;
        _laneMover = player.LaneMover;
    }

    public void Exit()
    {
        _player = null;
        _laneMover = null;
    }

    public void OnDamaged()
    {
        _player.EnterStunState();
    }

    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        _laneMover.MoveLane(-1, false);
    }

    public void OnMoveRight(InputAction.CallbackContext context)
    {
        _laneMover.MoveLane(1, false);
    }

    public void OnParry(InputAction.CallbackContext context)
    {
        _player.EnterParryState();
    }

    public void Update()
    {
        
    }
}

public class PlayerParryState : IPlayerState
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

    public void OnDamaged()
    {

    }

    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        _laneMover.MoveLane(-1, false, true);
    }

    public void OnMoveRight(InputAction.CallbackContext context)
    {
        _laneMover.MoveLane(1, false, true);
    }

    public void OnParry(InputAction.CallbackContext context)
    {
        return;
    }

    public void Update()
    {
    }

    private void EndParry()
    {
        _player.EnterDriveState();
    }
}

public class PlayerStunState : IPlayerState
{
    private Player _player;
    private InvincibleSystem _invincibleSystem;
    public void Enter(Player player)
    {
        _player = player;
        _invincibleSystem = _player.GetComponent<InvincibleSystem>();

        _player.StartStun();
        _invincibleSystem.OnFinishedInvincible += EndStun;

    }

    public void Exit()
    {
        _invincibleSystem.OnFinishedInvincible -= EndStun;
        _player = null;
    }

    public void OnDamaged()
    {
    }

    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        return;
    }

    public void OnMoveRight(InputAction.CallbackContext context)
    {
        return;
    }

    public void OnParry(InputAction.CallbackContext context)
    {
        return;
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

    public class NoInputState : IPlayerState
{
    public void Enter(Player player)
    {
        GameEvents.SetPhase(GamePhase.Shop, new PhaseData());
    }

    public void Exit()
    {
        
    }

    public void OnDamaged()
    {

    }

    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        return;
    }

    public void OnMoveRight(InputAction.CallbackContext context)
    {
        return;
    }

    public void OnParry(InputAction.CallbackContext context)
    {
        return;
    }

    public void Update()
    {
        return;
    }
}