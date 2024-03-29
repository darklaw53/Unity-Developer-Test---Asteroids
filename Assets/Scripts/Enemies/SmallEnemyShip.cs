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

    protected override void FireWeapon()
    {
        targetDirectionV3 = (CharacterControllerShip.Instance.transform.position - transform.position).normalized;

        Vector3 deviation = Quaternion.Euler(0, Random.Range(-maxDeviationAngle, maxDeviationAngle), 0) * targetDirectionV3;

        if (maxDeviationAngle == 0) deviation = targetDirectionV3;
        targeter.rotation = Quaternion.LookRotation(deviation);
        targetDirection = targeter.rotation;
        base.FireWeapon();
    }
}