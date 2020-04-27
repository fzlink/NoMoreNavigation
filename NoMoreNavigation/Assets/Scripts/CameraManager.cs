using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    private static readonly float ZoomSpeedMouse = 5f;
    private static readonly float[] ZoomBounds = new float[] { 10f, 85f };

    private static readonly float PanSpeed = 40f;
    private static readonly float[] BoundsX = new float[] { -25f, 500f };
    private static readonly float[] BoundsZ = new float[] { -50f, 500f };
    private Camera cam;
    private Vector3 clampedPos;
    private Vector3 lastPanPosition;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        //if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        //{
        //    clampedPos = transform.position;
        //    clampedPos -= Vector3.up * 2;
        //    ClampYPos();
        //}
        //else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        //{
        //    clampedPos = transform.position;
        //    clampedPos += Vector3.up * 2;
        //    ClampYPos();
        //}
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(scroll, ZoomSpeedMouse);

        if (Input.GetMouseButtonDown(2))
        {
            lastPanPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(2))
        {
            PanCamera(Input.mousePosition);
        }
    }

    private void PanCamera(Vector3 newPanPosition)
    {
        // Determine how much to move the camera
        Vector3 offset = cam.ScreenToViewportPoint(lastPanPosition - newPanPosition);
        Vector3 move = new Vector3(offset.x * PanSpeed, 0, offset.y * PanSpeed);

        // Perform the movement
        transform.Translate(move, Space.World);

        // Ensure the camera remains within bounds.
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, BoundsX[0], BoundsX[1]);
        pos.z = Mathf.Clamp(transform.position.z, BoundsZ[0], BoundsZ[1]);
        transform.position = pos;

        // Cache the position
        lastPanPosition = newPanPosition;
    }

    void ZoomCamera(float offset, float speed)
    {
        if (offset == 0)
        {
            return;
        }

        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - (offset * speed), ZoomBounds[0], ZoomBounds[1]);
    }

    private void ClampYPos()
    {
        clampedPos.y = Mathf.Clamp(clampedPos.y, 10f, 10000f);
        transform.position = clampedPos;
    }
}
