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
    public SpriteRenderer leftThruster, rightThruster, middleThruster, 
        leftStrafeThruster, rightStrafeThruster, retroThruster, retroThruster2;
    public Gun gun;

    [Header("OtherComponents")]
    public GameObject deathExplosion;
    public GameObject warpJump;
    public string ammoTag;

    //Private variables
    float horizontalImput, thrustInput;
    [HideInInspector] public bool _leftThrusterB, _rightThrusterB, _middleThrusterB,
        _leftStrafeThrusterB, _rightStrafeThrusterB, _retroThrusterB, _retroThruster2B;

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
        RotationAndStrafing();
        Thrust();
        Drift();
    }

    void Update()
    {
        DetectInput();
    }

    void DetectInput()
    {
        horizontalImput = -Input.GetAxis("Horizontal");
        thrustInput = Input.GetAxis("Vertical");
        
        if ((Input.GetButton("Fire1") || Input.GetButton("Jump")) || rotationSpeed == 0)
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

    void RotationAndStrafing()
    {
        if (horizontalImput != 0)
        {
            float rotation = horizontalImput * rotationSpeed * Time.deltaTime;
            if (rotationSpeed != 0) transform.Rotate(Vector3.forward * rotation);
            else
            {
                Vector2 thrustForce = transform.right * -horizontalImput * thrustSpeed;

                rb2D.AddForce(thrustForce);

                if (rb2D.velocity.magnitude > maxSpeed)
                {
                    rb2D.velocity = rb2D.velocity.normalized * maxSpeed;
                }
            }

            if (horizontalImput < 0)
            {
                if (_leftThrusterB) leftThruster.enabled = true;
                if (_leftStrafeThrusterB) leftStrafeThruster.enabled = true;
            }
            else if (horizontalImput > 0)
            {
                if (_rightThrusterB) rightThruster.enabled = true;
                if (_rightStrafeThrusterB) rightStrafeThruster.enabled = true;
            }
        }
    }

    void Thrust()
    {
        if (thrustInput > 0f || (thrustInput < 0f && (_retroThrusterB || _retroThruster2B)))
        {
            Vector2 thrustForce = transform.up * thrustInput * thrustSpeed;

            rb2D.AddForce(thrustForce);

            if (rb2D.velocity.magnitude > maxSpeed)
            {
                rb2D.velocity = rb2D.velocity.normalized * maxSpeed;
            }

            if (thrustInput > 0f)
            { 
                if (_leftThrusterB) leftThruster.enabled = true;
                if (_rightThrusterB) rightThruster.enabled = true;
                if (_middleThrusterB) middleThruster.enabled = true;
            }
            else
            {
                if (_retroThrusterB) retroThruster.enabled = true;
                if (_retroThruster2B) retroThruster2.enabled = true;
            }
        }
        else if (horizontalImput == 0)
        {
            if (_leftThrusterB) leftThruster.enabled = false;
            if (_rightThrusterB) rightThruster.enabled = false;
            if (_middleThrusterB) middleThruster.enabled = false;
            if (_retroThrusterB) retroThruster.enabled = false;
            if (_retroThruster2B) retroThruster2.enabled = false;
            if (_leftStrafeThrusterB) leftStrafeThruster.enabled = false;
            if (_rightStrafeThrusterB) rightStrafeThruster.enabled = false;
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