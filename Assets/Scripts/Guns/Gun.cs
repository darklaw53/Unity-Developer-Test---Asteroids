using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float fireCooldown = 0.1f;
    public GameObject ammunition;
    public Transform firePoint;

    float nextFireTime = 0f;

    public void Fire()
    {
        if (Time.time > nextFireTime)
        {
            nextFireTime = Time.time + fireCooldown;
            InstantiateAmmo();
        }
    }

    protected virtual void InstantiateAmmo()
    {
        var x = Instantiate(ammunition, firePoint.position, transform.rotation);
        x.GetComponent<Rigidbody2D>().velocity = CharacterControllerShip.Instance.rb2D.velocity;
    }
}