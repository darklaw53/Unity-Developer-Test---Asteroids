using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject ammunition;

    public void Fire()
    {
        var x = Instantiate(ammunition, transform.position, transform.rotation);
        x.GetComponent<Rigidbody2D>().velocity = CharacterControllerShip.Instance.rb2D.velocity;
    }
}