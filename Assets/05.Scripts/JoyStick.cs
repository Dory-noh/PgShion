using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform background;
    public RectTransform handle;

    public float radius = 200f;

    public Vector2 InputDir { get; private set; }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background,
            eventData.position,
            eventData.pressEventCamera,
            out pos
        );

        Vector2 dir = pos / radius;
        InputDir = Vector2.ClampMagnitude(dir, 1f);

        // 반원 제한
        if (InputDir.y < 0)
            InputDir = new Vector2(InputDir.x, 0);

        handle.anchoredPosition = InputDir * radius;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        InputDir = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }
}