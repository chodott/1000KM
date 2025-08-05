using UnityEngine;

public class TempObstacle : MonoBehaviour, IParryable
{
    private bool _isParried { get; set; }
    void IParryable.OnParried()
    {
        _isParried = true;
    }

    void Update()
    {
        if (_isParried)
        {
            transform.position = transform.position + transform.forward * Time.deltaTime;
        }
    }
}
