using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallEnemyShip : Enemy
{
    public float maxDeviationAngle = 10f;

    Vector3 targetDirectionV3;

    protected override void Start()
    {
        base.Start();
        targetRandomly = false;

        maxDeviationAngle = maxDeviationAngle - Mathf.Round(GameManager.Instance.score / 6000);
        if (maxDeviationAngle < 0) maxDeviationAngle = 0;
    }

    public override void FireWeapon()
    {
        var x = CharacterControllerShip.Instance.transform.position;
        var y = GameManager.Instance.currentLevel/2;
        if (y < 1) y = 1;
        if (y > 10) y = 10;
        float variability = 10 / (10*y);
        float minVar = -3 * variability;
        float maxVar = 4 * variability;
        targetDirectionV3 = (new Vector3(x.x + Random.Range(minVar, maxVar), 
            x.y + Random.Range(minVar, maxVar), 0) - transform.position).normalized; 

        var z = Instantiate(ammo, transform.position, targetDirection);
        z.GetComponent<Rigidbody2D>().velocity = rb2D.velocity;
        z.transform.up = targetDirectionV3;
    }
}