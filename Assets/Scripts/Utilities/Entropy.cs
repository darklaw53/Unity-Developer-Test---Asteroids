using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entropy : MonoBehaviour
{
    public float selfDestructTime = 5.0f;

    void Start()
    {
        Invoke("DestroyObject", selfDestructTime);
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}