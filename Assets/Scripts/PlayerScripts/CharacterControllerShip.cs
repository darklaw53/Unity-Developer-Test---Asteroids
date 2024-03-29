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

    [Header("Gun Settings")]
    public float fireCooldown = 0.1f; 

    [Header("Ship Components")]
    public Rigidbody2D rb2D;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer leftThruster, rightThruster;
    public Gun gun;

    [Header("OtherComponents")]
    public GameObject deathExplosion;
    public GameObject warpJump;
    public string ammoTag;

    //Private variables
    float rotationInput, thrustInput;
    float nextFireTime = 0f;

    private void Start()
    {
        //set ship variables from scriptableObject
        if (GameManager.Instance.shipLayoutSO != null)
        {
            rotationSpeed = GameManager.Instance.shipLayoutSO.rotationSpeed;
            thrustSpeed = GameManager.Instance.shipLayoutSO.thrustSpeed;
            driftAmount = GameManager.Instance.shipLayoutSO.driftAmount;
            spriteRenderer.sprite = GameManager.Instance.shipLayoutSO.sprite;
            var x = Instantiate(GameManager.Instance.shipLayoutSO.gunPrefab, gun.transform.parent);
            Destroy(gun.gameObject);
            gun = x.GetComponent<Gun>();
        }
    }

    void FixedUpdate()
    {
        //Physics updates
        Rotation();
        Thrust();
        Drift();
    }

    void Update()
    {
        DetectInput();
    }

    void DetectInput()
    {
        rotationInput = -Input.GetAxis("Horizontal");
        thrustInput = Input.GetAxis("Vertical");

        if ((Input.GetButton("Fire1") || Input.GetButton("Jump")) && Time.time > nextFireTime)
        {
            gun.Fire(); 
            nextFireTime = Time.time + fireCooldown;
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

    void Rotation()
    {
        float rotation = rotationInput * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.forward * rotation);

        if (rotationInput < 0) leftThruster.enabled = true;
        else if (rotationInput > 0) rightThruster.enabled = true;
    }

    void Thrust()
    {
        if (thrustInput > 0f)
        {
            Vector2 thrustForce = transform.up * thrustInput * thrustSpeed;

            rb2D.AddForce(thrustForce);

            if (rb2D.velocity.magnitude > maxSpeed)
            {
                rb2D.velocity = rb2D.velocity.normalized * maxSpeed;
            }

            leftThruster.enabled = true;
            rightThruster.enabled = true;
        }
        else if (rotationInput == 0)
        {
            leftThruster.enabled = false;
            rightThruster.enabled = false;
        }
    }

    void Drift()
    {
        Vector2 driftVelocity = -rb2D.velocity.normalized * driftAmount;
        rb2D.AddForce(driftVelocity);
    }

    public void Explode()
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
        if (collision.CompareTag(ammoTag))
        {
            Destroy(collision.gameObject);
            Explode();
        }
    }
}