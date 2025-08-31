using UnityEngine;
using UnityEngine.UI;

public class MoneyText : MonoBehaviour
{
    public PlayerStatus ps;
    [SerializeField] Text text;

    // Update is called once per frame
    void Update()
    {
        if (ps != null) text.text = ps.CurrentMoney + " ¿ø";
    }
}
