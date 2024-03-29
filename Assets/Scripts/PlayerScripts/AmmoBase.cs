using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBase : MonoBehaviour
{
    public float speed = 1f;
    public float lifetime = 3f;

    public Rigidbody2D rb2D;

    private void Start()
    {
        Invoke("DestroyAmmunition", lifetime);
        Impulse();
    }

    void Impulse()
    {
        if (rb2D != null)
        {
            Vector2 inheritedVelocity = CharacterControllerShip.Instance.rb2D.velocity;

            Vector2 forwardImpulse = transform.up * speed;

            rb2D.velocity = inheritedVelocity + forwardImpulse;
        }
    }

    private void DestroyAmmunition()
    {
        Destroy(gameObject);
    }
}
