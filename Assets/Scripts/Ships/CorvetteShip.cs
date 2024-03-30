using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorvetteShip : CharacterControllerShip
{
    [Header("Shields")]
    public GameObject shields;
    public AudioSource shieldSound;

    //internal
    bool shieldsUp = true;

    private void OnEnable()
    {
        shieldsUp = true;
        shields.SetActive(true);
    }

    public override void DetectInput()
    {
        horizontalImput = -Input.GetAxis("Horizontal");
        thrustInput = Input.GetAxis("Vertical");

        if ((Input.GetButton("Fire1") || Input.GetButton("Jump")))
        {
            gun.Fire();
        }
    }

    void RechargeShields()
    {
        shieldsUp = true;
        shields.SetActive(true);
    }

    public override void Explode()
    {
        if (shieldsUp)
        {
            shieldsUp = false;
            shields.SetActive(false);
            shieldSound.Play();
            Invoke("RechargeShields", 10);
        }
        else base.Explode();
    }
}