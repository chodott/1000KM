using UnityEngine;

public interface IState<in TOwner>
{
    public void Enter(TOwner owner);
    public void Exit();
    public void Update();
    public void OnParried(Vector3 contactPoint, float damage, float moveLaneSpeed);
    public void OnCollisionEnter(Collision collision);
    public void OnTriggerEnter(Collider other);
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

    public void OnParried(Vector3 contactPoint, float damage, float moveLaneSpeed)
    {
        _curState.OnParried(contactPoint, damage, moveLaneSpeed);
    }
    public void OnCollisionEnter(Collision collision)
    {
        if(_curState == null)
        {
            return;
        }    
        _curState.OnCollisionEnter(collision);
    }

    public void OnTriggerEnter(Collider other)
    {
        _curState.OnTriggerEnter(other);
    }

    public void Reset()
    {
        _curState?.Exit();
        _curState = null;
    }
}

public interface IEnemyState: IState<CarEnemy>
{

}


public class DriveState : IEnemyState
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
    }

    public void OnParried(Vector3 contactPoint, float damage, float moveLaneSpeed)
    {
        Vector3 parriedDirection = (contactPoint - _enemy.transform.position).normalized;

        float sign = Mathf.Sign(_enemy.transform.position.z - contactPoint.z);
        float angle = Vector3.Angle(parriedDirection, Vector3.back * sign);
        _enemy.TakeDamage(damage);
        bool isDead = _enemy.CheckDie(parriedDirection);
        if(isDead)
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
        if(other.attachedRigidbody.gameObject.TryGetComponent<IDamagable>(out var damagable))
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

public class VerticalKnockbackState : IEnemyState
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

        else if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (collision.gameObject.TryGetComponent<CarEnemy>(out var otherCar))
            {

                otherCar.ShareVelocity(otherCar);
            }
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

public class HorizontalKnockbackState : IEnemyState
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

public class DestroyedState : IEnemyState
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
        enemy.LaneMover.StopLaneMove();

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