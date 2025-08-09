using UnityEngine;

public class TitleObj : MonoBehaviour
{
    public bool isOnMouse;

    public void MouseEnter()
    {
        isOnMouse = true;
    }

    public void MouseExit()
    {
        isOnMouse = false;
    }
}


