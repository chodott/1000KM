using UnityEngine;

public class PartsButton : MonoBehaviour
{
    [SerializeField] PartStatus part;

    public void AddStatus()
    {
        part.AddStatus(part);
    }
}
