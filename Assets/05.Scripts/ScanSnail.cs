using UnityEngine;

public class ScanSnail : MonoBehaviour
{
    public float rotateSpeed = 200f;

    void Update()
    {
        if (Input.GetMouseButton(0)) 
        {
            float mouseX = Input.GetAxis("Mouse X");
            transform.Rotate(0f, -mouseX * rotateSpeed * Time.deltaTime, 0f);
        }
    }
}
