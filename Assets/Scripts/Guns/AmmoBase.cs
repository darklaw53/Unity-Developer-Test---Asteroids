using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBase : MonoBehaviour
{
    public float speed = 1f;
    public float lifetime = 3f;
    public string ammoTag;

    public Rigidbody2D rb2D;

    private void Start()
    {
        Invoke("DestroyAmmunition", lifetime);
        float zRotation = transform.rotation.eulerAngles.z;
        transform.rotation = Quaternion.Euler(0f, 0f, zRotation);
        Impulse();
    }

    //this is here so ships dont accidentally shoot themselves
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Admin"))
        {
            gameObject.tag = ammoTag;
        }
    }

    public virtual void Impulse()
    {
        if (rb2D != null)
        {
            rb2D.velocity = transform.up * speed + new Vector3 (rb2D.velocity.x, rb2D.velocity.y, 0);
        }
    }

    void DestroyAmmunition()
    {
        Destroy(gameObject);
    }
}