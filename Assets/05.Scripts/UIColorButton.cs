using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Drawing;

public class UIColorButton : MonoBehaviour, IPointerClickHandler
{
    public Material targetRenderer;
    private Image img;

    void Awake()
    {
        img = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        targetRenderer.SetColor("_BaseColor", img.color); 
    }
}