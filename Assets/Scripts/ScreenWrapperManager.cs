using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrapperManager : MonoBehaviour
{
    //private variables
    Camera mainCamera;
    Vector2 screenBounds;

    private void Awake()
    {
        mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
    }

    public void WrapObject(Transform objectTransform, bool horizontal)
    {
        Rigidbody2D rb = objectTransform.GetComponent<Rigidbody2D>();
        Vector3 newPosition = objectTransform.position;
        Vector2 velocity = rb.velocity;

        if (horizontal)
        {
            if (objectTransform.position.x > 0)
            {
                newPosition.x = -screenBounds.x + objectTransform.localScale.x / 3 + 0.001f;
            }
            else if (objectTransform.position.x < 0)
            {
                newPosition.x = screenBounds.x - objectTransform.localScale.x / 3 - 0.001f;
            }
        }
        else
        {
            if (objectTransform.position.y > 0)
            {
                newPosition.y = -screenBounds.y + objectTransform.localScale.y / 3 + 0.001f;
            }
            else if (objectTransform.position.y < 0)
            {
                newPosition.y = screenBounds.y - objectTransform.localScale.y / 3 - 0.001f;
            }
        }

        objectTransform.position = newPosition;
        rb.velocity = velocity;
    }
}