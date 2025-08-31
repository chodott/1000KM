using System.Collections;
using UnityEngine;

public class BurstAround : MonoBehaviour
{
    [SerializeField] private GameObject burstPrefab; 

    [SerializeField] private int burstCount = 8;
    [SerializeField] private Vector2 radiusRange = new(0.8f, 1.6f);
    [SerializeField] private Vector2 intervalRange = new(0.04f, 0.10f);
    [SerializeField] private bool faceOutward = true;

    private Coroutine _coroutineHandle;

    private void OnDisable()
    {
        Stop();
    }
    public void Play(Transform center = null)
    {
        _coroutineHandle = StartCoroutine(Co_Bursts());
    }

    public void Stop()
    {
        if (_coroutineHandle != null)
        {
            StopCoroutine(_coroutineHandle);
        }
    }

    private IEnumerator Co_Bursts()
    {
        for (int i = 0; i < burstCount; i++)
        {
            var center = gameObject.transform.position;
            var dir = Random.onUnitSphere; dir.y = Mathf.Clamp(dir.y, -0.2f, 0.7f);
            var pos = center + dir * Random.Range(radiusRange.x, radiusRange.y);
            var rot = faceOutward ? Quaternion.LookRotation(dir) : Random.rotation;

            var go = Instantiate(burstPrefab, pos, rot);
            foreach (var ps in go.GetComponentsInChildren<ParticleSystem>(true))
                ps.Play();

            // 수명 후 정리
            float maxLife = 0f;
            foreach (var ps in go.GetComponentsInChildren<ParticleSystem>(true))
            {
                var m = ps.main;
                float life = m.duration +
                             (m.startLifetime.mode == ParticleSystemCurveMode.Constant
                              ? m.startLifetime.constant
                              : m.startLifetime.constantMax);
                if (life > maxLife) maxLife = life;
            }
            Destroy(go, maxLife + 0.25f);

            yield return new WaitForSeconds(Random.Range(intervalRange.x, intervalRange.y));
        }
    }
}