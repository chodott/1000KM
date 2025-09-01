using JetBrains.Annotations;
using System;
using Unity.VisualScripting;
using UnityEngine;

public enum GamePhase
{
    Shop,
    BossIntro,
    Normal,
    GameOver
}

[System.Serializable]
public class PhaseData
{
    public float lockVelocity;
    public float duration;
    public Vector2 mapBendSize;
}

public static class GameEvents
{
    public static event Action<GamePhase, PhaseData> OnPhaseChanged;
    public static event Action OnBossDefeated;

    public static void SetPhase(GamePhase phase, PhaseData phaseData = null)
    {
        OnPhaseChanged?.Invoke(phase, phaseData);
    }
    public static void RaiseBossDefeated() => OnBossDefeated?.Invoke();
}