using UnityEngine;

public class BossDirector : MonoBehaviour
{
    [SerializeField]
    private PhaseData _bossPhaseData;
    [SerializeField]
    private float _bossInterval = 1000f;
    private float _bossSpawnDistance = 1000f;




    private void OnEnable()
    {
        GameEvents.OnBossDefeated += OnBossDefeated;
        _bossSpawnDistance = _bossInterval;
    }

    private void FixedUpdate()
    {
        if (_bossSpawnDistance <= GlobalMovementController.Instance.TotalDistance)
        {
            GameEvents.SetPhase(GamePhase.BossIntro, _bossPhaseData);
            _bossSpawnDistance += _bossInterval;
            CutSceneManager.instance.EnableBoss();
        }
    }

    private void OnBossDefeated()
    {
        GameEvents.SetPhase(GamePhase.Normal, new PhaseData());
        _bossSpawnDistance = GlobalMovementController.Instance.TotalDistance + _bossInterval;
    }

    public float GetDistanceToBoss()
    {
        float distance = _bossSpawnDistance - GlobalMovementController.Instance.TotalDistance;
        return distance;
    }
}