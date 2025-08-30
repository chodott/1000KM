using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CarEnemy : BaseEnemy, IPoolingObject, IParryable
{

    public static event Action<int> OnRewardDropped;

    #region SerializeField

    [SerializeField]
    private MeshFilter _meshFilter;
    [SerializeField]
    private MeshRenderer _meshRenderer;

    [SerializeField]
    private GameObject _destroyEffectPrefab;
    #endregion 

    private StateMachine<CarEnemy> _stateMachine;
    private EnemyColor _color;
    private EnemyStatData _statData;
    private GameObject _originalPrefab;

    private float _returnPositionX = 5f;
    private float _patternCooldownTime = 1;
    private float _patternCooldownTimer;

    public GameObject OriginalPrefab { get { return _originalPrefab; }}

    public event Action<GameObject, GameObject> OnReturned;

    public enum EnemyColor
    {
        White,
        Blue,
        Red,
        Yellow
    }

    #region Monobehaviour Callbacks

    void Awake()
    {
        _stateMachine = new StateMachine<CarEnemy>();
    }

    void OnEnable()
    {
        GlobalMovementController.Instance.OnReachedMaxDistance += SelfDestroy;
    }

    void OnDisable()
    {
        GlobalMovementController.Instance.OnReachedMaxDistance -= SelfDestroy;
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        _stateMachine.Update();

    }
    #endregion  

    public void ChangeState(IEnemyState state)
    {
        _stateMachine.ChangeState(state, this);
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

        LaneMover.CheckAndMoveLane(transform.position, _statData.ColliderSize, direction);
    }

    private void ResetPhysics()
    {
        Rb.angularVelocity = Vector3.zero;
        Rb.linearVelocity = Vector3.zero;
        Rb.Sleep();
    }

    private void SelfDestroy()
    {
        ChangeState(new DestroyedState(Vector3.up));
        Deactivate();
    }

    private void Destroy(Vector3 direction)
    {
        Collider.enabled = false;
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
        _curHealthPoint = statData.HealthPoint;
        _color = color;

        _meshFilter.mesh = statData.Mesh;
        _meshRenderer.material = _statData.materialVariants[(int)color];

        Collider.size = statData.ColliderSize;
        Collider.center = statData.ColliderCenter;
        Collider.enabled = true;

        transform.position = position;
        transform.rotation = statData.SpawnRotation;
        transform.localScale = statData.Scale;
        Rb.WakeUp();

        ChangeState(DriveState);
        LaneMover.Init(laneIndex);

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
        _curHealthPoint -= damage;

    }

    public bool CheckDie(Vector3 parriedDirection)
    {
        if(_curHealthPoint <= 0f)
        {
            Destroy(parriedDirection);
            return true;
        }
        return false;
    }

    public void RecoverVelocity()
    {
        if (_curVelocity <= GlobalMovementController.Instance.GlobalVelocity)
        {
            ChangeState(DriveState);
        }
    }

    public void ApplyExplosionForce(Vector3 direction)
    {
        Rb.AddForce(direction * _parryPower, ForceMode.Impulse);
        Rb.AddTorque(direction * _parryPower, ForceMode.Impulse);
    }

    public void SpawnDestroyEffect()
    {
        Instantiate(_destroyEffectPrefab, transform.position, Quaternion.identity);
    }

    public void ApplyAccident(Collision collision)
    {
        Vector3 forceDirection = Rb.position - collision.rigidbody.position;
        forceDirection.Normalize();
        Destroy(forceDirection);
    }

    public override void KnockbackToSide(Vector3 parriedDirection, float sign)
    {
        bool canMove = LaneMover.KnockbackLane(sign);
        if (canMove == false)
        {
            Destroy(parriedDirection);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        _stateMachine.OnCollisionEnter(collision);
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
        _stateMachine.Reset();
        OnReturned?.Invoke(OriginalPrefab,gameObject);
    }
    #endregion

    #region IParyable Callbacks
    public void OnParried(Vector3 contactPoint, float damage, float moveLaneSpeed)
    {
        _stateMachine.OnParried(contactPoint, damage, moveLaneSpeed);
    }

    public void OnAttack()
    {
        Rb.Sleep();
    }
    #endregion
}
