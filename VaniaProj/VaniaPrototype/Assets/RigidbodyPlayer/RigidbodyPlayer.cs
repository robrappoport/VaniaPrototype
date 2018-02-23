using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RigidbodyPlayer : MonoBehaviour
{

    private Rigidbody2D rb;
    public KeyCode[] myInputs;
    public float moveForce;
    public float initMoveForce;
    [Range(1, 20)]
    public float jumpVelocity;
    private float normJumpVel;
    public float fallMultiplier;
    public float lowJumpMultiplier;
    public bool CanJump;
    public bool Grounded;
    public float airborneHorizontalMovement;

    public float maxCharge;
    public float currentCharge;
    public bool onPlatform;
    public Image chargeImage;
    // Use this for initialization
    void Start()
    {
        currentCharge = maxCharge;
        normJumpVel = jumpVelocity;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerMovement();
        GravityMultiplier();
       
    }
    private void Update()
    {
        
        ChargeCheck();
        if (CanJump)
        {
            moveForce = initMoveForce;
            PlayerJump();
        }
        if (!CanJump && moveForce >= airborneHorizontalMovement)
        {
            moveForce = airborneHorizontalMovement;
        }
    }


    private void PlayerMovement()
    {
        if (Input.GetKey(myInputs[0]))
        {
            transform.Translate(-Vector2.right * (moveForce * Time.deltaTime));
        }
        if (Input.GetKey(myInputs[1]))
        {
            transform.Translate(Vector2.right * (moveForce * Time.deltaTime));
        }

    }

    private void GravityMultiplier()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;

        }
        else if (rb.velocity.y > 0 && !Input.GetKey(myInputs[2]))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void PlayerJump()
    {
        if (Input.GetKeyDown(myInputs[2]))
        {
            CanJump = false;
            rb.velocity = Vector2.up * jumpVelocity;
        }
    }
    public void ChargeCheck()
    {
        currentCharge = Mathf.Clamp(currentCharge, 0, maxCharge);
        chargeImage.fillAmount = currentCharge *.01f;
        if (!onPlatform)
        {
            currentCharge -= Time.deltaTime * 12;
            GameManager.instance.score--;
        }
        if (onPlatform)
        {
            currentCharge += Time.deltaTime * 10f;
        }

        if (currentCharge <= (maxCharge / 3))
        {
            jumpVelocity = (6);
        }
        else
        {
            jumpVelocity = normJumpVel;
        }

        if (currentCharge <= 0)
        {
            GameManager.instance.EndGame();
        }
    }

}
