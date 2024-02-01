using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControl : MonoBehaviour
{
    public Camera SceneCam;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Convert the mouse position to world coordinates
            Vector3 worldMousePosition = SceneCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));

            // Snap the object's X and Y coordinates to the mouse click position, keeping the Z-coordinate fixed
            transform.position = worldMousePosition;
        }

            
    }
}