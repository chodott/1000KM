using UnityEngine;

public class ShopSelectUI : MonoBehaviour
{
    [SerializeField] PlayerStatus ps;
    [SerializeField] GameObject resumeText;
    [SerializeField] AudioHoldCrossfade ac;

    private void OnEnable()
    {
        //prevTimeScale = Time.timeScale;
        Time.timeScale = 0f;
    }

    public void OnclickExitShopButton()
    {
        ps.RefillToilet();
        resumeText.SetActive(true);
        ac.ResumeFromHold();
    }
}
