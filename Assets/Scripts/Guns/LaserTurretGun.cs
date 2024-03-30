using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaserTurretGun : Gun
{
    protected override void InstantiateAmmo()
    {
        Vector3 direction = (FindClosestObjectWithTag().transform.position - firePoint.position).normalized;

        transform.up = direction;

        base.InstantiateAmmo();
    }

    public GameObject FindClosestObjectWithTag()
    {
        GameObject[] objectsWithTags = GameObject.FindGameObjectsWithTag("Asteroid")
            .Concat(GameObject.FindGameObjectsWithTag("Enemy")).ToArray();

        if (objectsWithTags.Length == 0)
        {
            return null;
        }

        GameObject closestObject = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject obj in objectsWithTags)
        {
            float distance = Vector3.Distance(currentPosition, obj.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestObject = obj;
            }
        }

        return closestObject;
    }
}