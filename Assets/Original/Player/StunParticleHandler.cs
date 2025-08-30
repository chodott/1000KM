using UnityEngine;

public class StunParticleHandler : MonoBehaviour
{
    [SerializeField]
    ParticleSystem _particleSystem;
    [SerializeField]
    private float _simulationSpeed = 1.5f;

    private ParticleSystem.MainModule _main;

    public void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _main = _particleSystem.main;
    }

    public void PlayStunEffect()
    {
        if(_particleSystem.isStopped == false)
        {
            return;
        }
        _main.simulationSpeed = _simulationSpeed;
        _particleSystem.Play(true);
    }

    public void StopStunEffect()
    {
        _particleSystem.Stop();
    }
}
