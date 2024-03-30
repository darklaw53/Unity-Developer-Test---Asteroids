using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonScraperGun : Gun
{
    protected override void InstantiateAmmo()
    {
        var x = Instantiate(ammunition, firePoint.position, transform.rotation);
        x.transform.parent = transform;
    }
}