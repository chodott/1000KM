using UnityEngine;


abstract public class StateEvent 
{
    
}

sealed class ParriedEvent: StateEvent
{

}

sealed class InputMoveEvent : StateEvent
{
    public int IsRight { get; }
    public InputMoveEvent(int isRight) => IsRight = isRight;
}

sealed class OnDamagedEvent:StateEvent
{
}

sealed class OnCollisionEvent : StateEvent
{
    public Collision Collision { get; }
    public OnCollisionEvent(Collision collision) => Collision = collision;
}

sealed class OnTriggerEvent : StateEvent
{
    public Collider Collider { get;}
    public OnTriggerEvent(Collider collider) => Collider = collider;
}

public interface IState<in TOwner>
{
    public void Enter(TOwner owner);
    public void Exit();
    public void Update();
    public void HandleEvent(StateEvent stateEvent);
}

public class StateMachine<TOwner> where TOwner : MonoBehaviour
{
    IState<TOwner> _curState;

    public void ChangeState(IState<TOwner> nextState, TOwner owner)
    {
        _curState?.Exit();
        _curState = nextState;
        _curState.Enter(owner);
    }

    public void Update()
    {
        _curState.Update();
    }

    public void GenerateStateEvent(StateEvent stateEvent)
    {
        _curState.HandleEvent(stateEvent);
    }

    public void Reset()
    {
        _curState?.Exit();
        _curState = null;
    }
}

public class DriveState : IState<CarEnemy>
{
    private CarEnemy _enemy;
    public void Enter(CarEnemy enemy)
    {
        _enemy = enemy;
        _enemy.LaneMover.UpdateMoveLaneSpeed(0f);
        _enemy.gameObject.layer = LayerMask.NameToLayer("Enemy");

    }

    public void Exit()
    {
        _enemy.ResetPatternTimer();
        _enemy = null;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
        {
            _enemy.ApplyAccident(collision);
        }

        else if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (collision.gameObject.TryGetComponent<CarEnemy>(out var otherCar))
            {
                if (otherCar.Velocity > _enemy.Velocity)
                {
                    return;
                }
                otherCar.ApplyAccident(collision);
            }
        }
    }

    public void OnParried(Vector3 contactPoint, float damage, float moveLaneSpeed)
    {
        Vector3 parriedDirection = (contactPoint - _enemy.transform.position).normalized;

        float sign = Mathf.Sign(_enemy.transform.position.z - contactPoint.z);
        float angle = Vector3.Angle(parriedDirection, Vector3.back * sign);
        _enemy.TakeDamage(damage);
        bool isDead = _enemy.CheckDie(parriedDirection);
        if (isDead)
        {
            return;
        }
        if (angle >= 85.0f)
        {
            _enemy.ChangeState(new VerticalKnockbackState());
        }
        else
        {
            _enemy.ChangeState(new HorizontalKnockbackState(parriedDirection, moveLaneSpeed, sign));
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody.gameObject.TryGetComponent<IDamagable>(out var damagable))
        {
            bool result = damagable.OnDamaged(10);
            if (result)
            {
                _enemy.ExcludeCollision();
            }
        }
    }

    public void Update()
    {
        _enemy.CheckPatternTimer();
        _enemy.ApplyFriction();
        _enemy.CheckOutInRange();
    }
}

public class VerticalKnockbackState : IState<CarEnemy>
{
    private CarEnemy _enemy;
    public void Enter(CarEnemy enemy)
    {
        _enemy = enemy;
        _enemy.gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");
        _enemy.KnockbackToForward();
    }

    public void Exit()
    {
        _enemy = null;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
        {
            _enemy.ApplyAccident(collision);
        }
    }

    public void OnParried(Vector3 contactPoint, float damage, float moveLaneSpeed)
    {

    }

    public void OnTriggerEnter(Collider other)
    {
    }

    public void Update()
    {
        _enemy.ApplyFriction();
        _enemy.RecoverVelocity();
    }
}

public class HorizontalKnockbackState : IState<CarEnemy>
{
    private CarEnemy _enemy;
    private LaneMover _laneMover;
    private Vector3 _knockbackDirection;
    private float _knockbackSpeed;
    private float _laneDirection;
    public HorizontalKnockbackState(Vector3 knockbackDirection, float moveLaneSpeed, float laneDirection)
    {
        _knockbackDirection = knockbackDirection;
        _knockbackSpeed = moveLaneSpeed;
        _laneDirection = laneDirection;
    }
    public void Enter(CarEnemy enemy)
    {
        _enemy = enemy;
        _enemy.gameObject.layer = LayerMask.NameToLayer("Enemy");
        _laneMover = enemy.LaneMover;
        _laneMover.UpdateMoveLaneSpeed(_knockbackSpeed);
        _laneMover.OnFinishMove += EndKnockback;

        _enemy.KnockbackToSide(_knockbackDirection, _laneDirection);
    }

    public void Exit()
    {
        _laneMover.OnFinishMove -= EndKnockback;
        _laneMover = null;
        _enemy = null;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
        {
            _enemy.ApplyAccident(collision);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
    }

    public void OnParried(Vector3 contactPoint, float damage, float moveLaneSpeed)
    {
        return;
    }

    public void Update()
    {
        _enemy.ApplyFriction();
    }

    public void EndKnockback()
    {
        _enemy.ChangeState(_enemy.DriveState);
    }
}

public class DestroyedState : IState<CarEnemy>
{
    private CarEnemy _enemy;
    private Vector3 _explosionDirection;

    public DestroyedState(Vector3 explosionDirection)
    {
        _explosionDirection = explosionDirection;
    }
    public void Enter(CarEnemy enemy)
    {
        _enemy = enemy;
        _enemy.ResetVelocity();
        _enemy.ApplyExplosionForce(_explosionDirection);
        _enemy.SpawnDestroyEffect();
    }

    public void Exit()
    {
        _enemy = null;
    }

    public void OnCollisionEnter(Collision collision)
    {
        return;
    }

    public void OnTriggerEnter(Collider other)
    {
    }

    public void OnParried(Vector3 contactPoint, float damage, float moveLaneSpeed)
    {
        return;
    }

    public void Update()
    {

    }
}