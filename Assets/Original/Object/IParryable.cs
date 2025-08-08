using UnityEngine;

public interface IParryable
{
    public void OnParried(Vector3 direction, float force, float damage);
}
