using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrapperManager : MonoBehaviour
{
    public BoxCollider2D boxCollider2D;

    //private variables
    Camera mainCamera;
    Vector2 screenBounds;

    private void Awake()
    {
        mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        ScaleCollider();
    }

    void ScaleCollider()
    {
        if (boxCollider2D != null)
        {
            boxCollider2D.size = new Vector2(screenBounds.x * 2f + .1f, screenBounds.y * 2f + .1f);
        }
        else
        {
            Debug.LogError("No BoxCollider2D attached to the GameObject.");
        }
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
                newPosition.x = -screenBounds.x - objectTransform.localScale.x / 2;
            }
            else if (objectTransform.position.x < 0)
            {
                newPosition.x = screenBounds.x + objectTransform.localScale.x / 2;
            }
        }
        else
        {
            if (objectTransform.position.y > 0)
            {
                newPosition.y = -screenBounds.y - objectTransform.localScale.y / 2;
            }
            else if (objectTransform.position.y < 0)
            {
                newPosition.y = screenBounds.y + objectTransform.localScale.y / 2;
            }
        }

        objectTransform.position = newPosition;
        rb.velocity = velocity;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Transform objectTransform = collision.transform;
        Vector3 objectPosition = objectTransform.position;

        if (objectPosition.x > screenBounds.x)
        {
            // Object exited through the right
            WrapObject(objectTransform, true);
        }
        else if (objectPosition.x < -screenBounds.x)
        {
            // Object exited through the left
            WrapObject(objectTransform, true);
        }
        else if (objectPosition.y > screenBounds.y)
        {
            // Object exited through the top
            WrapObject(objectTransform, false);
        }
        else if (objectPosition.y < -screenBounds.y)
        {
            // Object exited through the bottom
            WrapObject(objectTransform, false);
        }
    }
}