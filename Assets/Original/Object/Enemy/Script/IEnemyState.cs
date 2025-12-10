using UnityEngine;

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

    public void HandleEvent(StateEvent stateEvent)
    {
        switch (stateEvent)
        {
            case ProjectileHitEvent projectileHitEvent:
                _enemy.ApplyAccident(projectileHitEvent.Collision);
                break;

            case CarCollisionEvent carCollisionEvent:
                Collision collision = carCollisionEvent.Collision;
                if (collision.gameObject.TryGetComponent<CarEnemy>(out var otherCar))
                {
                    if (otherCar.Velocity > _enemy.Velocity)
                    {
                        return;
                    }
                    otherCar.ApplyAccident(collision);
                }
                break;

            case ParriedEvent parriedEvent:
                _enemy.TakeDamage(parriedEvent.Damage);
                Vector3 parriedDirection = (parriedEvent.ContactPoint - _enemy.transform.position).normalized;
                float sign = Mathf.Sign(_enemy.transform.position.z - parriedEvent.ContactPoint.z);
                float angle = Vector3.Angle(parriedDirection, Vector3.back * sign);
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
                    _enemy.ChangeState(new HorizontalKnockbackState(parriedDirection, parriedEvent.MoveLaneSpeed, sign));
                }
                break;

            case OnTriggerEnterEvent triggerEnterEvent:
                if (triggerEnterEvent.Collider.attachedRigidbody.gameObject.TryGetComponent<IDamagable>(out var damagable))
                {
                    bool result = damagable.OnDamaged(10);
                    if (result)
                    {
                        _enemy.ExcludeCollision();
                    }
                }
                break;
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

    public void HandleEvent(StateEvent stateEvent)
    {
        switch (stateEvent)
        {
            case ProjectileHitEvent projectileHitEvent:
                _enemy.ApplyAccident(projectileHitEvent.Collision);
                break;
        }
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

    public void HandleEvent(StateEvent stateEvent)
    {
        switch (stateEvent)
        {
            case ProjectileHitEvent projectileHitEvent:
                _enemy.ApplyAccident(projectileHitEvent.Collision);
                break;
        }
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

    public void HandleEvent(StateEvent stateEvent)
    {
    }

    public void Update()
        {

        }
    }