using System;
using UnityEngine;

public class RestAreaEntrance : MonoBehaviour
{
    [SerializeField]
    private BoxCollider _boxCollider;
    [SerializeField]
    private Vector3 _spawnPosition;
    [SerializeField]
    private float _deactivatePosX = 5;

    private float _returnPosX;

    public event Action OnDeactivated;
    public float SpawnPosX { get { return _spawnPosition.x; } }
    private void Awake()
    {
        _returnPosX = _boxCollider.size.x / 2 + _deactivatePosX;
    }

    private void Update()
    {
        float curVelocity = GlobalMovementController.Instance.GlobalVelocity;
        transform.position += (Vector3.right * curVelocity * Time.deltaTime);

        if(_returnPosX <= transform.position.x)
        {
            Deactivate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Player>(out Player player))
        {
            player.EnterNoInputState();
            CutSceneManager.instance.EnableShop();
            Deactivate();
        }
    }
    public void Activate()
    {
        gameObject.SetActive(true);
        _boxCollider.enabled = true;
        transform.position = _spawnPosition;
    }

    private void Deactivate()
    {
        OnDeactivated?.Invoke();
        gameObject.SetActive(false);
    }
}