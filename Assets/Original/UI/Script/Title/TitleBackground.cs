using UnityEngine;

public class TitleBackground : MonoBehaviour
{
    public float colorChangeSpeed = 1f; // 색 변환 속도 (초당 진행률)

    private Camera cam;
    private Color[] colors;
    private int currentIndex = 0;
    private int nextIndex = 1;
    private float t = 0f;

    void Start()
    {
        cam = GetComponent<Camera>();

        // 색상 순서 설정
        colors = new Color[]
        {
            Color.red,
            Color.yellow,
            Color.green,
            Color.blue,
            new Color(0.5f, 0f, 0.5f) // 보라색
        };

        cam.backgroundColor = colors[0];
    }

    void Update()
    {
        t += Time.deltaTime * colorChangeSpeed;

        cam.backgroundColor = Color.Lerp(colors[currentIndex], colors[nextIndex], t);

        if (t >= 1f)
        {
            t = 0f;
            currentIndex = nextIndex;
            nextIndex = (nextIndex + 1) % colors.Length;
        }
    }
}
