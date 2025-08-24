using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    public PartStatus partStatus;
    public int payment;
    Text buttonText;

    private void Start()
    {
        buttonText = GetComponentInChildren<Text>();
        buttonText.text = payment + " ¿ø";
    }

    public void OnClickOilButton()
    {

    }

    public void OnClickRepairButton()
    {

    }

    public void OnClickPartButton()
    {

    }
}
