using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public interface IEnemyState
{
    public void Enter(CarEnemy enemy);
    public void Exit();

    public void Update();

    public void OnParried(Vector3 contactPoint, float damage, float moveLaneSpeed);

    public void OnCollisionEnter(Collision collision);
}


public class DriveState : IEnemyState
{
    private CarEnemy _enemy;
    public void Enter(CarEnemy enemy)
    {
        _enemy = enemy;
    }

    public void Exit()
    {
        _enemy.ResetPatternTimer();
        _enemy = null;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Parried"))
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

        if (angle >= 80.0f)
        {
            _enemy.KnockbackToForward();
        }
        else
        {
            _enemy.UpdateMoveLaneSpeed(moveLaneSpeed);
            _enemy.KnockbackToSide(parriedDirection, sign);
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
    }

    public void Exit()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Parried"))
        {
            _enemy.ApplyAccident(collision);
        }
    }

    public void OnParried(Vector3 contactPoint, float damage, float moveLaneSpeed)
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
        _enemy.ApplyFriction();
        _enemy.RecoverVelocity();
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
    }

    public void Exit()
    {
        _enemy = null;
    }

    public void OnCollisionEnter(Collision collision)
    {
        return;
    }

    public void OnParried(Vector3 contactPoint, float damage, float moveLaneSpeed)
    {
        return;
    }

    public void Update()
    {
        _enemy.CheckOutInRange();
    }


}