using UnityEngine;

public interface IParryable
{
    public void OnParried(Vector3 contactPosition,  float damage);
    public void OnAttack();
}
