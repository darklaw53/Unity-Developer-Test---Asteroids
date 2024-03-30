using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerShip : Singleton<CharacterControllerShip>
{
    [Header("Movement Settings")]
    public float rotationSpeed = 180f;
    public float thrustSpeed = 5f;
    public float maxSpeed = 15;

    [Header("Drift Settings")]
    public float driftAmount = 0.1f;

    [Header("Ship Components")]
    public Rigidbody2D rb2D;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer leftThruster, rightThruster;
    public Gun gun;

    [Header("OtherComponents")]
    public GameObject deathExplosion;
    public GameObject warpJump;

    //Private variables
    [HideInInspector] public float horizontalImput, thrustInput;

    private void Start()
    {
        //set ship variables from scriptableObject
        if (GameManager.Instance.shipLayoutSO != null)
        {
            spriteRenderer.sprite = GameManager.Instance.shipLayoutSO.sprite;
        }
    }

    void FixedUpdate()
    {
        //Physics updates
        RotationAndStrafing();
        Thrust();
        Drift();
    }

    void Update()
    {
        DetectInput();
    }

    public virtual void DetectInput()
    {
        horizontalImput = -Input.GetAxis("Horizontal");
        thrustInput = Input.GetAxis("Vertical");
        
        if ((Input.GetButton("Fire1") || Input.GetButton("Jump")))
        {
            gun.Fire(); 
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            WarpJump();
        }
    }

    void WarpJump()
    {
        Instantiate(warpJump, transform.position, transform.rotation);
        GameManager.Instance.StartCoroutine("WarpJump");
        gameObject.SetActive(false);
    }

    public virtual void RotationAndStrafing()
    {
        if (horizontalImput != 0)
        {
            float rotation = horizontalImput * rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.forward * rotation);

            if (horizontalImput < 0)
            {
                leftThruster.gameObject.SetActive(true);
            }
            else if (horizontalImput > 0)
            {
                rightThruster.gameObject.SetActive(true);
            }
        }
    }

    public virtual void Thrust()
    {
        if (thrustInput > 0f)
        {
            Vector2 thrustForce = transform.up * thrustInput * thrustSpeed;

            rb2D.AddForce(thrustForce);

            if (rb2D.velocity.magnitude > maxSpeed)
            {
                rb2D.velocity = rb2D.velocity.normalized * maxSpeed;
            }

            leftThruster.gameObject.SetActive(true);
            rightThruster.gameObject.SetActive(true);
        }
        else if (horizontalImput == 0)
        {
            leftThruster.gameObject.SetActive(false);
            rightThruster.gameObject.SetActive(false);
        }
    }

    void Drift()
    {
        Vector2 driftVelocity = -rb2D.velocity.normalized * driftAmount;
        rb2D.AddForce(driftVelocity);
    }

    public virtual void Explode()
    {
        GameManager.Instance.lives--;
        GameManager.Instance.LooseALife();

        Instantiate(deathExplosion, transform.position, transform.rotation);

        if (GameManager.Instance.lives <= 0) GameManager.Instance.GameOver();
        else GameManager.Instance.StartCoroutine("Exploded");
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ammunition"))
        {
            Destroy(collision.gameObject);
            Explode();
        }
    }
}