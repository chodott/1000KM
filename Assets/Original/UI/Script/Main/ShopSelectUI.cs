using UnityEngine;

public class ShopSelectUI : MonoBehaviour
{
    [SerializeField] PlayerStatus ps;
    private float prevTimeScale;

    private void OnEnable()
    {
        prevTimeScale = Time.timeScale;
        Time.timeScale = 0f;
    }

    public void OnclickExitShopButton()
    {
        ps.RefillToilet();
        Time.timeScale = prevTimeScale;
    }
}
