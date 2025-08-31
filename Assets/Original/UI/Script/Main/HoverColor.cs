using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image targetImage;
    private Color original;
    AudioSource audioSource;

    void Start()
    {
        original = targetImage.color;
        audioSource = GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetImage.color = Color.black;
        audioSource.Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetImage.color = original;
    }
}