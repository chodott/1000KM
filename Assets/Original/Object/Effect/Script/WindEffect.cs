using UnityEngine;

public class WindEffect : MonoBehaviour
{
    [SerializeField]
    ParticleSystem _particleSystem;
    [SerializeField]
    private float _minStartSpeed = 9f;
    [SerializeField] 
    private float _maxStartSpeed = 11f;
    [SerializeField]
    private float _minRateOverTime;
    [SerializeField] 
    private float _maxRateOverTime;

    [SerializeField]
    private float _minApplyVelocity = 20f;
    [SerializeField]
    private float _maxApplyVelocity = 150f;


    private void OnEnable()
    {
        var particleRenderer = _particleSystem.GetComponent<ParticleSystemRenderer>();
    }

    private void Update()
    {

        float curVelocity = GlobalMovementController.Instance.GlobalVelocity;
        var emission = _particleSystem.emission;
        var mainMoudle = _particleSystem.main;

        float curRateOverTime = 0;
        float curStartSpeed = 0;
        if (curVelocity >= _minStartSpeed)
        {
            float lerpAlpha = Mathf.InverseLerp(_minApplyVelocity, _maxApplyVelocity, curVelocity);
            curRateOverTime =  Mathf.Lerp(_minRateOverTime, _maxRateOverTime, lerpAlpha);
            curStartSpeed = Mathf.Lerp(_minStartSpeed, _maxStartSpeed, lerpAlpha);
        }

        emission.rateOverTime = curRateOverTime;
        mainMoudle.startSpeed = curStartSpeed;
    }

}
