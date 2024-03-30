using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinShip : CharacterControllerShip
{
    public SpriteRenderer middleThruster;

    public override void DetectInput()
    {
        horizontalImput = -Input.GetAxis("Horizontal");
        thrustInput = Input.GetAxis("Vertical");

        if ((Input.GetButton("Fire1") || Input.GetButton("Jump")))
        {
            gun.Fire();
        }
    }

    public override void Thrust()
    {
        if (thrustInput > 0f)
        {
            Vector2 thrustForce = transform.up * thrustInput * thrustSpeed;

            rb2D.AddForce(thrustForce);

            if (rb2D.velocity.magnitude > maxSpeed)
            {
                rb2D.velocity = rb2D.velocity.normalized * maxSpeed;
            }

            middleThruster.gameObject.SetActive(true);
        }
        else
        {
            middleThruster.gameObject.SetActive(false);
        }
    }

    public override void RotationAndStrafing()
    {
        if (horizontalImput != 0)
        {
            float rotation = horizontalImput * rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.forward * rotation);
        }
    }
}