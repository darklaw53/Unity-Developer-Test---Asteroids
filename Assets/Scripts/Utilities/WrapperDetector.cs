using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapperDetector : MonoBehaviour
{
    public ScreenWrapperManager screenWrapperManager;
    public bool horizontal;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        screenWrapperManager.WrapObject(collision.transform, horizontal);
    }
}