using UnityEngine;

public class CameraFOVController : MonoBehaviour
{
    [SerializeField] 
    private Camera mainCam;
    [SerializeField] 
    private float minFOV = 60f;
    [SerializeField] 
    private float maxFOV = 80f;
    [SerializeField] 
    private float lerpSpeed = 5f;
    [SerializeField]
    public float maxSpeed = 200f;
    [SerializeField]
    public float minSpeed = 60f;

    void Update()
    {
        float playerSpeed = GlobalMovementController.Instance.GlobalVelocity;
        float targetFOV = minFOV;
        if (playerSpeed > minSpeed)
        {
            float t = Mathf.InverseLerp(minSpeed, maxSpeed, playerSpeed);
            targetFOV = Mathf.Lerp(minFOV, maxFOV, t);
        }

        mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, targetFOV, Time.deltaTime * lerpSpeed);
    }
}