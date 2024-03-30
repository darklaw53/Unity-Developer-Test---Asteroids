using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float speed = 1f;
    public Rigidbody2D rb2D;
    public int pointValue;

    public string ammoTag;

    public GameObject smallerAsteroids;

    private void Start()
    {
        Impulse();
    }

    void Impulse()
    {
        if (rb2D != null)
        {
            rb2D.velocity = transform.up * speed + new Vector3(rb2D.velocity.x, rb2D.velocity.y, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(ammoTag))
        {
            Destroy(collision.gameObject);
            Split();
        }

        if (collision.CompareTag("MoonBeam"))
        {
            Split();
        }

        if (collision.CompareTag("Player"))
        {
            CharacterControllerShip.Instance.Explode();
            Split();
        }

        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().Die();
            Split();
        }
    }

    void Split()
    {
        GameManager.Instance.GainPoints(pointValue);

        if (smallerAsteroids != null)
        {
            var x = Random.Range(-0.5f, 0.5f);
            var y = Random.Range(-0.5f, 0.5f);
            GameObject asteroid1 = Instantiate(smallerAsteroids, transform.position + new Vector3(x, y, 0), Quaternion.identity);
            GameObject asteroid2 = Instantiate(smallerAsteroids, transform.position - new Vector3(x, y, 0), Quaternion.identity);

            Vector3 direction = (asteroid2.transform.position - asteroid1.transform.position).normalized;

            asteroid1.transform.up = -direction;
            asteroid2.transform.up = direction;
        }

        AsteroidManager.Instance.AsteroidDestroyed();

        //addscore
        Destroy(gameObject);
    }
}
