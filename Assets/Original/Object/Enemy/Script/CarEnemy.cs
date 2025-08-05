using UnityEngine;

public class CarEnemy : MonoBehaviour,IParryable
{
    //Temp Show Inspector
    [SerializeField]
    private EnemyStatData _statData;
    private IEnemyBehavoiur _enemyBehavoiur;
    private Rigidbody _rigidbody;
    private bool _isParried;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

    }

    void Update()
    {
        if (_isParried == false)
        {
            float curVelocity = _statData.Velocity - GlobalMovementController.Instance.globalVelocity ;
            transform.position += (transform.forward * curVelocity * Time.deltaTime);
        }
    }

    public void OnParried(Vector3 force)
    {
        Debug.Log($"Parry!!!! :  {force}");
        _isParried = true;
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;
        _rigidbody.AddTorque(force, ForceMode.Impulse);
    }
}
