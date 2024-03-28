using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerShip : MonoBehaviour
{
    [Header("Movement Settings")]
    public float rotationSpeed = 180f;
    public float thrustSpeed = 5f;

    [Header("Drift Settings")]
    public float driftAmount = 0.1f;

    [Header("Ship Components")]
    public Rigidbody2D rb;

    //Private variables
    float rotationInput, thrustInput;

    void FixedUpdate()
    {
        //Physics updates
        Rotation();
        Thrust();
        Drift();
    }

    void Update()
    {
        DetectInput();
    }

    void DetectInput()
    {
        rotationInput = -Input.GetAxis("Horizontal");
        thrustInput = Input.GetAxis("Vertical");
    }

    void Rotation()
    {
        float rotation = rotationInput * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.forward * rotation);
    }

    void Thrust()
    {
        if (thrustInput != 0f)
        {
            rb.AddForce(transform.up * thrustInput * thrustSpeed);
        }
    }

    void Drift()
    {
        Vector2 driftVelocity = -rb.velocity.normalized * driftAmount;
        rb.AddForce(driftVelocity);
    }
}