using UnityEngine;

public class UIMove : MonoBehaviour
{
    public float speed = 100f; // 초당 이동 속도 (픽셀 단위)

    RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        // 왼쪽(-x) 방향으로 이동
        rect.anchoredPosition += Vector2.left * speed * Time.deltaTime;
    }
}
