using UnityEngine;

public class PlayerParrySystem : MonoBehaviour
{
    [SerializeField]
    private BoxCollider _boxCollider;
    [SerializeField]
    private float _parryCooldownTime = 0.1f;

    private float _parryCooldownTimer;
    private bool _canParry = true;


    // Update is called once per frame
    void Update()
    {
        UpdateParryTime();
    }

    #region Collider Callbacks
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Parry Success");
        if (other.TryGetComponent<IParryable>(out var parryable))
        {
            parryable.OnParried();
        }
    }
    #endregion

    private void UpdateParryTime()
    {
        if (_canParry == false)
        {
            _parryCooldownTimer += Time.deltaTime;
            if (_parryCooldownTimer > _parryCooldownTime)
            {
                _parryCooldownTimer = 0.0f;
                _canParry = true;
            }
        }
    }

    public void Parry()
    {
        if (_canParry == true)
        {
            _boxCollider.enabled = true;
            _canParry = false;
        }
    }
}
