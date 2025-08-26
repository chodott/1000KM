using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CarEnemy : MonoBehaviour,IParryable, IPoolingObject
{

    public static event Action<int> OnRewardDropped;

    #region SerializeField
    [SerializeField]
    private Rigidbody _rigidbody;
    [SerializeField]
    private LaneMover _laneMover;
    [SerializeField]
    private MeshFilter _meshFilter;
    [SerializeField]
    private MeshRenderer _meshRenderer;
    [SerializeField]
    private BoxCollider _collider;
    [SerializeField]
    private GameObject _destroyEffectPrefab;
    #endregion 

    private IEnemyState _curState;
    private EnemyColor _color;
    private EnemyStatData _statData;
    private GameObject _originalPrefab;
    private Vector3 _forwardVector = new Vector3(-1, 0, 0);
    private float _returnPositionX = 5f;
    private float _patternCooldownTime = 1;
    private float _patternCooldownTimer;
    private float _healthPoint;
    private float _curVelocity;
    private float _maxVelocity;

    private float _friction = 1f;
    private float _knockbackPower = 0.5f;
    private float _parryPower = 50f;


    private DriveState _driveState = new DriveState();

    public DriveState DriveState  { get{ return _driveState; } }

    public GameObject OriginalPrefab { get { return _originalPrefab; }}
    public LaneMover LaneMover { get { return _laneMover; }}

    public event Action<GameObject, GameObject> OnReturned;

    public enum EnemyColor
    {
        White,
        Blue,
        Red,
        Yellow
    }

    #region Monobehaviour Callbacks


    void FixedUpdate()
    {
        MoveForward(_curVelocity);
        _curState.Update();
    }
    #endregion  

    private void MoveForward(float velocity)
    {
        float curVelocity = velocity - GlobalMovementController.Instance.GlobalVelocity;
        Vector3 targetPosition = _rigidbody.position + (_forwardVector * curVelocity * Time.fixedDeltaTime);
        _rigidbody.MovePosition(targetPosition);
    }

    public void ChangeState(IEnemyState state)
    {
        if(_curState != null)
        {
            _curState.Exit();
        }
        _curState = state;
        _curState.Enter(this);
    }

    private void DoPattern()
    {
        int direction;
        switch(_color)
        {
            case EnemyColor.White:
                direction = 0;
                break;

             case EnemyColor.Blue:
                direction = -1;
                break;

             case EnemyColor.Red:
                direction = 1;
                break;
            case EnemyColor.Yellow:
                direction = UnityEngine.Random.Range(0, 3) - 1;
                break;
            default:
                direction = 0;
                break;
        }

        _laneMover.CheckAndMoveLane(transform.position, _statData.ColliderSize, direction);
    }

    private void ResetPhysics()
    {
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.Sleep();
    }

    private void Destroy(Vector3 direction)
    {
        _collider.enabled = false;
        OnRewardDropped?.Invoke(1);
        Vector3 explosionDirection = new Vector3(-direction.x, 0.3f, direction.z);
        explosionDirection.Normalize();
        ChangeState(new DestroyedState(explosionDirection));
        StartCoroutine(DestroyAfterDelay(1f));
    }


    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Deactivate();
    }


    public void Init(EnemyColor color, EnemyStatData statData, Vector3 position, float initVelocity, int laneIndex)
    {
        gameObject.SetActive(true);

        _maxVelocity = initVelocity;
        _curVelocity = initVelocity;
        _statData = statData;
        _healthPoint = statData.HealthPoint;
        _color = color;

        _meshFilter.mesh = statData.Mesh;
        _meshRenderer.material = _statData.materialVariants[(int)color];

        _collider.size = statData.ColliderSize;
        _collider.center = statData.ColliderCenter;
        _collider.enabled = true;

        transform.position = position;
        transform.rotation = statData.SpawnRotation;
        transform.localScale = statData.Scale;
        _rigidbody.WakeUp();

        ChangeState(_driveState);
        _laneMover.Init(laneIndex);

    }

    public void CheckOutInRange()
    {
        if (transform.position.x >= _returnPositionX)
        {
            Deactivate();
        }
    }

    public void CheckPatternTimer()
    {
        if (_patternCooldownTimer > _patternCooldownTime)
        {
            DoPattern();
            _patternCooldownTimer = 0f;
        }
        else
        {
            _patternCooldownTimer += Time.deltaTime;
        }
    }

    public void ResetPatternTimer()
    {
        _patternCooldownTimer = 0f;
    }

    public void TakeDamage(float damage)
    {
        _healthPoint -= damage;

    }

    public bool CheckDie(Vector3 parriedDirection)
    {
        if(_healthPoint <= 0f)
        {
            Destroy(parriedDirection);
            return true;
        }
        return false;
    }

    public void ApplyFriction()
    {
        if (_curVelocity <= _maxVelocity)
        {
            _curVelocity = _maxVelocity;
        }
        else
        {
            _curVelocity -= _friction * Time.fixedDeltaTime * _curVelocity;
        }
    }

    public void RecoverVelocity()
    {
        if (_curVelocity <= GlobalMovementController.Instance.GlobalVelocity)
        {
            ChangeState(_driveState);
        }
    }

    public void ResetVelocity()
    {
        _curVelocity = 0f;
    }

    public void ApplyExplosionForce(Vector3 direction)
    {
        _rigidbody.AddForce(direction * _parryPower, ForceMode.Impulse);
        _rigidbody.AddTorque(direction * _parryPower, ForceMode.Impulse);
    }

    public void SpawnDestroyEffect()
    {
        Instantiate(_destroyEffectPrefab, transform.position, Quaternion.identity);
    }

    public void KnockbackToForward()
    {
        _curVelocity += (GlobalMovementController.Instance.GlobalVelocity * (1 + _knockbackPower));
    }

    public void KnockbackToSide(Vector3 parriedDirection, float sign)
    {
        bool canMove = _laneMover.KnockbackLane(sign);
        if (canMove == false)
        {
            Destroy(parriedDirection);
        }
    }

    public void ApplyAccident(Collision collision)
    {
        Vector3 forceDirection = _rigidbody.position - collision.rigidbody.position;
        forceDirection.Normalize();
        Destroy(forceDirection);
    }

    public void OnCollisionEnter(Collision collision)
    {
        _curState.OnCollisionEnter(collision);
    }

    #region PoolingObject Callbacks
    public void Activate(GameObject originalPrefab)
    {
        _originalPrefab = originalPrefab;
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        ResetPhysics();
        _curState = null;
        OnReturned?.Invoke(OriginalPrefab,gameObject);
    }
    #endregion

    #region IParyable Callbacks
    public void OnParried(Vector3 contactPoint, float damage, float moveLaneSpeed)
    {
        _curState.OnParried(contactPoint, damage, moveLaneSpeed);
    }

    public void OnAttack()
    {
        _rigidbody.Sleep();
    }
    #endregion
}
