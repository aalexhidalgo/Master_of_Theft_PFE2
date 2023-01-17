using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float RotationSpeed = 400f;

    public Transform playerOrientation;
    float yRotation, xRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
    }

    // Update is called once per frame
    void Update()
    {
        //Mouse Rotation
        float HorizontalInputMouse = Input.GetAxisRaw("Mouse X") * Time.deltaTime * RotationSpeed;
        float VerticalInputMouse = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * RotationSpeed;

        yRotation += HorizontalInputMouse;
        xRotation -= VerticalInputMouse;

        //Camera Rotation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        playerOrientation.rotation = Quaternion.Euler(0, yRotation, 0);

        //Rotation Limits
        xRotation = Mathf.Clamp(xRotation, -90f, 0f);
    }
}
