using UnityEngine;

public class LaneSystem : MonoBehaviour
{

    [SerializeField]
    private float _laneWidth = 3.0f;
    [SerializeField]
    private int _laneRange = 1;


    public float LaneWidth { get { return _laneWidth; } }
    public int LaneRange {  get { return _laneRange; } }
    public int LaneCount { get { return _laneRange * 2 + 1; } }

    public static LaneSystem Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    public bool GetCanMove(int direction, int curLaneIndex)
    {
        int maxLaneIndex = direction * _laneRange;
        if (curLaneIndex == maxLaneIndex)
        {
            return false;
        }

        return true;
    }

    public int GetLastLaneIndex(int direction)
    {
        int lastLaneIndex = direction * _laneRange;
        return lastLaneIndex;
    }

    public float GetLanePositionZ(int index)
    {
        return index * _laneWidth;
    }

}
