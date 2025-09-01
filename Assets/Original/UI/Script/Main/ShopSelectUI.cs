using UnityEngine;

public class ShopSelectUI : MonoBehaviour
{
    [SerializeField] PlayerStatus ps;
    [SerializeField] GameObject resumeText;

    private void OnEnable()
    {
        //prevTimeScale = Time.timeScale;
        Time.timeScale = 0f;
    }

    public void OnclickExitShopButton()
    {
        ps.RefillToilet();
        resumeText.SetActive(true);
    }
}
