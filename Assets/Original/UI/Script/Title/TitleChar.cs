using UnityEngine;
using System.Collections;

public class TitleChar : MonoBehaviour
{
    public float delayTime = 2f;   // 움직이기 전 대기 시간
    public float moveSpeed = 2f;   // 위아래 이동 속도
    public float moveHeight = 1f;  // 이동 범위 (높이)

    private Vector3 startPos;
    private bool canMove = false;
    private float elapsedTime = 0f;

    void Start()
    {
        startPos = transform.position;
        StartCoroutine(StartMoveAfterDelay());
    }

    IEnumerator StartMoveAfterDelay()
    {
        yield return new WaitForSeconds(delayTime);
        canMove = true;
        elapsedTime = 0f; // 움직이기 시작할 때 시간 초기화
    }

    void Update()
    {
        if (!canMove) return;

        elapsedTime += Time.deltaTime;
        float y = Mathf.Sin(elapsedTime * moveSpeed) * moveHeight;
        transform.position = startPos + new Vector3(0, y, 0);
    }
}
