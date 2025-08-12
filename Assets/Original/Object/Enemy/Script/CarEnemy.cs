using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CarEnemy : MonoBehaviour,IParryable, IPoolingObject
{

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


    private EnemyState _enemyState;
    private EnemyColor _color;
    private EnemyStatData _statData;
    private GameObject _originalPrefab;
    private Vector3 _forwardVector = new Vector3(-1, 0, 0);
    private float _returnPositionX = 5f;
    private float _patternCooldownTime = 1;
    private float _patternCooldownTimer;
    private float _healthPoint;
    private float _curVelocity;

    private float _friction = 1f;
    private float _knockbackPower = 0.5f;
    private float _parryPower = 50f;

    public GameObject OriginalPrefab { get { return _originalPrefab; }}

    public event System.Action<GameObject, GameObject> OnReturned;

    public enum EnemyColor
    {
        White,
        Grey,
        Black,
        Yellow
    }

    public enum EnemyState
    {
        Drive,
        Knockback,
        Destroyed,
        NotUse
    }

    void Update()
    {
        if(_enemyState == EnemyState.NotUse)
        {
            return;
        }

        if(transform.position.x >=_returnPositionX)
        {
            Deactivate();
        }
    }

    void FixedUpdate()
    {

        switch(_enemyState)
        {
            case EnemyState.Drive:
                MoveForward(_curVelocity);
                CheckPattern();
                break;

             case EnemyState.Knockback:
                MoveForward(_curVelocity);
                break;

             case EnemyState.Destroyed:
                MoveForward(0f);
                break;
        }

        
    }

    private void MoveForward(float velocity)
    {
        float curVelocity = velocity - GlobalMovementController.Instance.globalVelocity;
        Vector3 targetPosition = _rigidbody.position + (_forwardVector * curVelocity * Time.fixedDeltaTime);
        _rigidbody.MovePosition(targetPosition);

        RecoverVelocity();
    }

    private void CheckPattern()
    {
        if (_patternCooldownTimer > _patternCooldownTime)
        {
            DoPattern();
            _patternCooldownTimer = 0;
        }
        else
        {
            _patternCooldownTimer += Time.deltaTime;
        }
    }

    private void RecoverVelocity()
    {
        
        if (_curVelocity <= _statData.Velocity)
        {
            _curVelocity = _statData.Velocity;
        }
        else
        {
            _curVelocity -= _friction * Time.fixedDeltaTime * _curVelocity;
        }

        if (_curVelocity <= GlobalMovementController.Instance.globalVelocity)
        {
            _enemyState = EnemyState.Drive;
        }


    }

    private void DoPattern()
    {
        int direction = 1;
        switch(_color)
        {
            case EnemyColor.White:
                direction = 0;
                break;

             case EnemyColor.Grey:
                direction = -1;
                break;

             case EnemyColor.Black:
                direction = 1;
                break;
            case EnemyColor.Yellow:
                direction = Random.Range(0, 3) - 1;
                break;
        }

        _laneMover.CheckAndMoveLane(transform.position, _statData.ColliderSize, direction);

    }

    private void Destroy(Vector3 direction)
    {
        _collider.enabled = false;
        
        Vector3 explosionDirection = new Vector3(-direction.x, 0.3f, direction.z);
        explosionDirection.Normalize();
        _rigidbody.AddForce(explosionDirection * _parryPower, ForceMode.Impulse);
        _rigidbody.AddTorque(explosionDirection *_parryPower, ForceMode.Impulse);
        _enemyState = EnemyState.Destroyed;

        StartCoroutine(DestroyAfterDelay(1f));
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Deactivate();
    }

    public void Init(EnemyColor color, EnemyStatData statData, Vector3 position, int laneIndex)
    {
        gameObject.SetActive(true);

        _statData = statData;
        _curVelocity = statData.Velocity;
        _healthPoint = statData.HealthPoint;
        _color = color;
        _meshFilter.mesh = statData.Mesh;
        _meshRenderer.material = _statData.materialVariants[(int)color];
        transform.localScale = statData.Scale;
        _collider.size = statData.ColliderSize;
        _collider.center = statData.ColliderCenter;
        _collider.enabled = true;

        transform.position = position;
        transform.rotation = statData.SpawnRotation;
        _rigidbody.WakeUp();

        _enemyState = EnemyState.Drive;
        _laneMover.Init(laneIndex);

    }
    public void OnParried(Vector3 contactPoint, float damage)
    {
        if (_enemyState != EnemyState.Drive)
        {
            return;
        }

        Vector3 parriedDirection = (contactPoint - transform.position).normalized;

        float sign = Mathf.Sign(transform.position.z - contactPoint.z);
        float angle = Vector3.Angle(parriedDirection, Vector3.back * sign);

        _healthPoint -= damage;
        if (_healthPoint <= 0f)
        {   //Destroy
            Destroy(parriedDirection);
        }
        else
        {   //Knockback

            if(angle >= 80.0f)
            {
                gameObject.tag = "Parried";
                _curVelocity = _statData.Velocity + (GlobalMovementController.Instance.globalVelocity * (1 + _knockbackPower));
                _enemyState = EnemyState.Knockback;
            }
            else
            {
                gameObject.tag = "Parried";
                bool moveResult = _laneMover.MoveLane(sign);
                if(moveResult == false)
                {
                    Destroy(parriedDirection);
                }
            }

        }
    }

    public void OnAttack()
    {
        _rigidbody.Sleep();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Parried"))
        {
            Vector3 forceDirection = _rigidbody.position - collision.rigidbody.position;
            forceDirection.Normalize();
            Destroy(forceDirection);
        }
    }

    #region PoolingObject Callbacks
    public void Activate(GameObject originalPrefab)
    {
        _originalPrefab = originalPrefab;
        _rigidbody.Sleep();

    }

    public void Deactivate()
    {
        _enemyState = EnemyState.NotUse;
        _rigidbody.Sleep();
        OnReturned?.Invoke(OriginalPrefab,gameObject);
    }
    #endregion
}
