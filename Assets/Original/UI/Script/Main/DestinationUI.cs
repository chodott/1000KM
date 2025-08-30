using UnityEngine;
using UnityEngine.UI;

public class DestinationUI : MonoBehaviour
{
    public Text finalText;
    public Text restText;
    public GlobalMovementController gc;
    float restDest = 500;

    void Update()
    {
        finalText.text = Mathf.FloorToInt((1000000 - gc.TotalDistance)/1000) + " KM";
        restText.text = Mathf.FloorToInt((restDest - gc.TotalDistance% restDest)) + " M";
    }
}
