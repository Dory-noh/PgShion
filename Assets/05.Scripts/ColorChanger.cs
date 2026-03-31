using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public Material targetRenderer;

    public void ChangeColor(Color color)
    {
        targetRenderer.SetColor("_BaseColor", color); 
    }
}