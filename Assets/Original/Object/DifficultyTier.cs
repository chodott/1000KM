
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[System.Serializable]
public struct DifficultyTier
{
    public float spawnRate;

    public float minVelocity;
    public float maxVelocity;

    public float laneCooldown;

    public Vector2Int spawnColorRange;
    public Vector2Int spawnStatRange;
}