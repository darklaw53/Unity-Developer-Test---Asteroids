using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class Enemy : MonoBehaviour
{
    [Header("Weapon Variables")]
    public float fireInterval = 1;
    public GameObject ammo;
    public Transform targeter;

    [Header("Time Between Direction Change")]
    public float changeIntervalMin = 1;
    public float changeIntervalMax = 5;

    [Header("")]
    public float speed = 3f;
    public string ammoTag;
    public Rigidbody2D rb2D;
    public GameObject deathExplosion;

    bool flyRight;
    [HideInInspector] public bool targetRandomly;
    [HideInInspector] public Quaternion targetDirection;

    protected virtual void Start()
    {
        targetRandomly = true;
        flyRight = Random.value > 0.5f;
        StartCoroutine(ChangeDirectionRoutine());
        StartCoroutine(FireGun());
    }

    IEnumerator ChangeDirectionRoutine()
    {
        while (true)
        {
            Impulse();
            yield return new WaitForSeconds(Random.Range(changeIntervalMin, changeIntervalMax));
        }
    }

    IEnumerator FireGun()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireInterval);
            FireWeapon();
        }
    }

    void Impulse()
    {
        if (rb2D != null)
        {
            Vector2 impulse;

            if (flyRight) impulse = transform.right * speed;
            else impulse = -transform.right * speed;

            bool diagonal = Random.value > 0.5f;

            if (diagonal)
            {
                bool up = Random.value > 0.5f;

                if (up) impulse = new Vector2(impulse.x, impulse.y + transform.up.y * speed);
                else impulse = new Vector2(impulse.x, impulse.y - transform.up.y * speed);
            }

            rb2D.velocity = impulse;
        }
    }

    //kills player if hits player directly but doesnt die, dies and destroys asteroids it hits
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(ammoTag))
        {
            Destroy(collision.gameObject);
            Die();
        }

        if (collision.CompareTag("Player"))
        {
            CharacterControllerShip.Instance.Explode();
        }

        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().Die();
            Die();
        }
    }

    public void Die()
    {
        Instantiate(deathExplosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    protected virtual void FireWeapon()
    {
        if (targetRandomly)
        {
            float randomRotation = Random.Range(0f, 360f);
            Quaternion rotation = Quaternion.Euler(0, 0, randomRotation);
            targetDirection = rotation;
        }
        var x = Instantiate(ammo, transform.position, targetDirection);
        x.GetComponent<Rigidbody2D>().velocity = rb2D.velocity;
    }
}