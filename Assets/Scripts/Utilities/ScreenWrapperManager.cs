using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrapperManager : MonoBehaviour
{
    public BoxCollider2D boxCollider2D;
    public bool backup;

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
            if (!backup) boxCollider2D.size = new Vector2(screenBounds.x * 2f + .1f, screenBounds.y * 2f + .1f);
            else boxCollider2D.size = new Vector2(screenBounds.x * 2f + 5f, screenBounds.y * 2f + 5f);
        }
        else
        {
            Debug.LogError("No BoxCollider2D attached to the GameObject.");
        }
    }

    public void WrapObject(Transform objectTransform, bool horizontal, bool vertical)
    {
        if (!backup)
        {
            Rigidbody2D rb = objectTransform.GetComponent<Rigidbody2D>();
            Vector3 newPosition = objectTransform.position;
            Vector2 velocity = rb.velocity;

            if (vertical)
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

            if (horizontal)
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
        else //for the backup colider to catch asteroids that slip out
        {
            Vector3 newPosition = Vector3.zero;
            GameObject obj = objectTransform.gameObject;

            int edgeIndex = Random.Range(0, 4);
            switch (edgeIndex)
            {
                case 0: // Top edge
                    newPosition = new Vector3(Random.Range(-screenBounds.x, screenBounds.x), screenBounds.y + obj.transform.localScale.y / 1.5f, 0f);
                    break;
                case 1: // Bottom edge
                    newPosition = new Vector3(Random.Range(-screenBounds.x, screenBounds.x), -screenBounds.y - obj.transform.localScale.y / 1.5f, 0f);
                    break;
                case 2: // Left edge
                    newPosition = new Vector3(-screenBounds.x - obj.transform.localScale.y / 1.5f, Random.Range(-screenBounds.y, screenBounds.y), 0f);
                    break;
                case 3: // Right edge
                    newPosition = new Vector3(screenBounds.x + obj.transform.localScale.y / 1.5f, Random.Range(-screenBounds.y, screenBounds.y), 0f);
                    break;
            }

            Vector3 directionToCenter = (new Vector3(Random.Range(-3, 4), Random.Range(-3, 4), 0) - newPosition).normalized;
            obj.transform.position = newPosition;
            obj.transform.up = directionToCenter;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Transform objectTransform = collision.transform;
        Vector3 objectPosition = objectTransform.position;
        bool vert = false;
        bool hor = false;

        if (objectPosition.x > screenBounds.x)
        {
            // Object exited through the right
            vert = true;
        }
        else if (objectPosition.x < -screenBounds.x)
        {
            // Object exited through the left
            vert = true;
        }
        else if (objectPosition.y > screenBounds.y)
        {
            // Object exited through the top
            hor = true;
        }
        else if (objectPosition.y < -screenBounds.y)
        {
            // Object exited through the bottom
            hor = true;
        }

        WrapObject(objectTransform, hor, vert);
    }
}