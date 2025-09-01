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
        particleRenderer.lengthScale = 0.2f;
    }

    private void Update()
    {

        float curVelocity = GlobalMovementController.Instance.GlobalVelocity;
        float lerpAlpha = Mathf.InverseLerp(_minApplyVelocity, _maxApplyVelocity, curVelocity);

        var emission = _particleSystem.emission;
        emission.rateOverTime = Mathf.Lerp(_minRateOverTime, _maxRateOverTime, lerpAlpha);
        var mainMoudle = _particleSystem.main;
        mainMoudle.startSpeed = Mathf.Lerp(_minStartSpeed, _maxStartSpeed, lerpAlpha);
    }

}
