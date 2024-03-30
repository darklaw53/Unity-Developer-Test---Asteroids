using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbeShip : CharacterControllerShip
{
    public SpriteRenderer middleThruster, leftStrafeThruster, rightStrafeThruster, retroThruster;

    public override void DetectInput()
    {
        horizontalImput = -Input.GetAxis("Horizontal");
        thrustInput = Input.GetAxis("Vertical");

        gun.Fire();
    }

    public override void Thrust()
    {
        if (thrustInput > 0f || thrustInput < 0f)
        {
            Vector2 thrustForce = transform.up * thrustInput * thrustSpeed;

            rb2D.AddForce(thrustForce);

            if (rb2D.velocity.magnitude > maxSpeed)
            {
                rb2D.velocity = rb2D.velocity.normalized * maxSpeed;
            }

            if (thrustInput > 0f)
            {
                middleThruster.enabled = true;
            }
            else
            {
                retroThruster.enabled = true;
            }
        }
        else if (horizontalImput == 0)
        {
            middleThruster.enabled = false;
            retroThruster.enabled = false;
            leftStrafeThruster.enabled = false;
            rightStrafeThruster.enabled = false;
        }
    }

    public override void RotationAndStrafing()
    {
        if (horizontalImput != 0)
        {
            Vector2 thrustForce = transform.right * -horizontalImput * thrustSpeed;

            rb2D.AddForce(thrustForce);

            if (rb2D.velocity.magnitude > maxSpeed)
            {
                rb2D.velocity = rb2D.velocity.normalized * maxSpeed;
            }

            if (horizontalImput < 0)
            {
                leftStrafeThruster.enabled = true;
            }
            else
            {
                rightStrafeThruster.enabled = true;
            }
        }
    }
}