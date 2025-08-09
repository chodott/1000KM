using UnityEngine;

public class TitleWheel : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0f, 100f, 0f); // 초당 회전 속도

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
