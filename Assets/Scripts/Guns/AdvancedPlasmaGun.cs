using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedPlasmaGun : Gun
{
    //second gun
    public Transform firePoint2;

    protected override void InstantiateAmmo()
    {
        var x = Instantiate(ammunition, firePoint.position, transform.rotation);
        x.GetComponent<Rigidbody2D>().velocity = CharacterControllerShip.Instance.rb2D.velocity;

        var y = Instantiate(ammunition, firePoint2.position, transform.rotation);
        y.GetComponent<Rigidbody2D>().velocity = CharacterControllerShip.Instance.rb2D.velocity;
    }
}