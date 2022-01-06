using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public static Vector3 Position { get; private set; }

    private Vector2 finishPos;
    private bool savedPos;

    private void Start()
    {
        Position = transform.position;
        savedPos = false;
    }

    private void LateUpdate()
    {
        Vector3 currentPos = Position;

        if (Progression.TimeOver && !savedPos)
        {
            finishPos = currentPos;
            savedPos = true;
        }

        if (savedPos)
        {
            if (PlayerController.Position.x - finishPos.x >= 3)
                return;
        }

        // Clamp camera position to player along the x-axis
        currentPos.x = Mathf.Clamp(transform.position.x, PlayerController.Position.x, currentPos.x);
        Position = currentPos;
        transform.position = Position;
    }
}
