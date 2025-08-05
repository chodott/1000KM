using AmazingAssets.CurvedWorld.Examples;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public RoadSpawner spawner;


    void Update()
    {
        transform.Translate(spawner.moveDirection * spawner.movingSpeed * Time.deltaTime);
    }
    void FixedUpdate()
    {
        switch (spawner.axis)
        {
            case RoadSpawner.Axis.XPositive:
                if (transform.position.x > spawner.destoryZone)
                    spawner.DestroyChunk(this);
                break;

            case RoadSpawner.Axis.XNegative:
                if (transform.position.x < -spawner.destoryZone)
                    spawner.DestroyChunk(this);
                break;

            case RoadSpawner.Axis.ZPositive:
                if (transform.position.z > spawner.destoryZone)
                    spawner.DestroyChunk(this);
                break;

            case RoadSpawner.Axis.ZNegative:
                if (transform.position.z < -spawner.destoryZone)
                    spawner.DestroyChunk(this);
                break;
        }

    }
}
