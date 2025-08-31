using UnityEngine;

public interface IParryable
{
    public void OnParried(Vector3 contactPosition,  float damage, float moveLaneSpeed);
}
