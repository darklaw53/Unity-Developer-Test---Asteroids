using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallEnemyShip : Enemy
{
    //direction target for weapon is
    Vector3 targetDirectionV3;

    protected override void Start()
    {
        base.Start();
    }

    public override void FireWeapon()
    {
        var playerPosition = CharacterControllerShip.Instance.transform.position;
        var level = Mathf.Clamp(GameManager.Instance.currentLevel / 2, 1, 10);
        float variability = 10 / (10 * level);
        float minVar = -3 * variability;
        float maxVar = 4 * variability;

        targetDirectionV3 = (new Vector3(playerPosition.x + Random.Range(minVar, maxVar),
            playerPosition.y + Random.Range(minVar, maxVar), 0) - transform.position).normalized; 

        var x = Instantiate(ammo, transform.position, targetDirection);
        x.GetComponent<Rigidbody2D>().velocity = rb2D.velocity;
        x.transform.up = targetDirectionV3;
    }
}