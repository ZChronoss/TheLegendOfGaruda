using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScreenButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public Image buttonImage;
    public Sprite idle;
    public Sprite pressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonImage.sprite = pressed;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonImage.sprite = idle;
    }
}
