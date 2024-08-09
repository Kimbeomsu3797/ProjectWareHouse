using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    Vector3 camPos;
    public float rotSpeed = 200f;
    float mouseX = 0;
    float mouseY = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouse_X = Input.GetAxis("Mouse X");
        float mouse_Y = Input.GetAxis("Mouse Y");
        mouseX += mouse_X * rotSpeed * Time.deltaTime;
        mouseY += mouse_Y * rotSpeed * Time.deltaTime;
        mouseY = Mathf.Clamp(mouseY, -30f, 30f);
        transform.eulerAngles = new Vector3(-mouseX, mouseY, 0);
    }
}
