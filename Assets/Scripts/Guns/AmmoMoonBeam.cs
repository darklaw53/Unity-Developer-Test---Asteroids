using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoMoonBeam : AmmoBase
{
    private void Awake()
    {
        gameObject.tag = ammoTag;
    }

    public override void Impulse()
    {
        //dont
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        //activate tag on awake instead
    }
}