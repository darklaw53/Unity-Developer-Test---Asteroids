using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorvetteShip : CharacterControllerShip
{
    public override void DetectInput()
    {
        horizontalImput = -Input.GetAxis("Horizontal");
        thrustInput = Input.GetAxis("Vertical");

        if ((Input.GetButton("Fire1") || Input.GetButton("Jump")))
        {
            gun.Fire();
        }
    }
}