using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCameraLogic : MonoBehaviour
{
    public float moveSpeed = 5.0f; // Speed at which the camera moves
    public float sensitivity = 2.0f; // Mouse sensitivity
    private Vector3 lastMousePosition; // Store the last mouse position

    private void LateUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            lastMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(1))
        {
            Vector3 mouseDelta = (Input.mousePosition - lastMousePosition) * sensitivity;
            transform.Rotate(-mouseDelta.y, mouseDelta.x, 0);
            lastMousePosition = Input.mousePosition;

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput) * moveSpeed * Time.fixedDeltaTime;
        transform.Translate(moveDirection);
    }
}
