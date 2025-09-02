using UnityEngine;
using UnityEngine.UI;

public class DestinationUI : MonoBehaviour
{
    public Text finalText;
    public Text restText;
    public GlobalMovementController globalMovementController;
    public BossDirector bossDirector;

    void Update()
    {
        float leftDistanceToRest = globalMovementController.GetDistanceToRest();
        float leftDistanceToBoss = bossDirector.GetDistanceToBoss();


        finalText.text = Mathf.FloorToInt((leftDistanceToBoss)) + " M";
        restText.text = Mathf.FloorToInt((leftDistanceToRest)) + " M";
    }
}
