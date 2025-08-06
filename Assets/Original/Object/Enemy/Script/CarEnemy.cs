using UnityEngine;

public class CarEnemy : MonoBehaviour,IParryable
{
    enum EnemyColor
    {
        White,
        Grey,
        Black,
        Yellow
    }

    //Temp Show Inspector
    [SerializeField]
    private EnemyStatData _statData;
    [SerializeField]
    private LaneMover _laneMover;
    [SerializeField]
    private Rigidbody _rigidbody;
    [SerializeField]
    private EnemyColor _color;

    private float _patternCooldownTime = 1;
    private float _patternCooldownTimer;
    private bool _isParried;

    void Update()
    {
        if (_isParried == false)
        {
            float curVelocity = _statData.Velocity - GlobalMovementController.Instance.globalVelocity ;
            transform.position += (transform.forward * curVelocity * Time.deltaTime);

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

        _laneMover.MoveLane(direction);

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
