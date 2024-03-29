using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject ammunition;

    public void Fire()
    {
        Instantiate(ammunition, transform.position, transform.rotation);
    }
}