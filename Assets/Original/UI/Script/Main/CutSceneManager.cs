using UnityEngine;

public class CutSceneManager : MonoBehaviour
{
    public static CutSceneManager instance;

    public GameObject shopCutscene;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void EnableShop()
    {
        shopCutscene.SetActive(true);
    }
}
