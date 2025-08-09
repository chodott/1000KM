using UnityEngine;

public class TitleButton : MonoBehaviour
{
    public GameObject obj;
    TitleObj titleObj;

    private void Start()
    {
        titleObj = GetComponent<TitleObj>();
    }

    private void Update()
    {
        if(titleObj.isOnMouse)
        {
            obj.SetActive(true);
        }
        else
        {
            obj.SetActive(false);
        }
    }
}
