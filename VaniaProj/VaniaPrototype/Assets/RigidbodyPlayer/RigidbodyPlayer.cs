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
    public float coyoteCounter = .05f;
    public float fallMultiplier;
    public float lowJumpMultiplier;
    public bool CanJump;
    public bool Grounded;
    public float airborneHorizontalMovement;
    public bool facingRight;
    public float maxCharge, smallMaxCharge;
    public float currentCharge;
    public bool onCharger, haveTouchedPlatform = false;
    public Image chargeImage;


    public float airDashTimer = 0;
    [Header("Can Use Specials")]
    public bool canDoubleJump = false;
    public bool canAirDash = false;

    // Use this for initialization
    void Start()
    {
        currentCharge = maxCharge;
        normJumpVel = jumpVelocity;
        rb = GetComponent<Rigidbody2D>();
        chargeImage = GameObject.Find("fill").GetComponent<Image>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GravityMultiplier();
       
    }
    private void Update()
    {
        //Debug.Log("velocity: " + rb.velocity);
        PlayerMovement();
        ChargeCheck();

        if (GameManager.instance.airDashEnabled)
        {
            Debug.Log("can air dash");
            if (canAirDash && Input.GetKey(myInputs[3]))
            {
                Debug.Log("air dashing");
                AirDash();
            }
        }
            PlayerJump();
        if (!CanJump && moveForce >= airborneHorizontalMovement)
        {
            moveForce = airborneHorizontalMovement;
        }
        if (airDashTimer >= 0f)
        {
            airDashTimer -= Time.deltaTime;
            if (airDashTimer <= 0f)
            {
                rb.gravityScale = 1f;
            }
        }

    }


    private void PlayerMovement()
    {
        if (airDashTimer <= 0f)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            if (Input.GetKey(myInputs[0]))
            {
                rb.velocity += new Vector2(-moveForce, 0);
                facingRight = false;
            }
            if (Input.GetKey(myInputs[1]))
            {
                rb.velocity += new Vector2(moveForce, 0);
                facingRight = true;
            }
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
            if (CanJump || coyoteCounter >= 0f)
            {
                moveForce = initMoveForce;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.velocity = Vector2.up * jumpVelocity;
                if (GameManager.instance.doubleJumpEnabled == true)
                {
                    canDoubleJump = true;
                }
            }
            else
            {
                if (canDoubleJump)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.velocity = Vector2.up * jumpVelocity;
                    canDoubleJump = false;
                }
              
            }
        }
    }
    public void ChargeCheck()
    {
       
        chargeImage.fillAmount = currentCharge *.01f;
        if (!onCharger)
        {
            currentCharge -= Time.deltaTime * 12;
            GameManager.instance.currentCharge--;
            haveTouchedPlatform = false;
        }
        if (onCharger)
        {
            if (!haveTouchedPlatform)
            {
                smallMaxCharge = currentCharge;
                haveTouchedPlatform = true;
            }
            if (GameManager.instance.currentActiveCharger.bigCharger)
            {
                currentCharge = Mathf.Clamp(currentCharge, 0, maxCharge);
                currentCharge += Time.deltaTime * 10f;  
            }
            else
            {
                currentCharge = Mathf.Clamp(currentCharge, 0, smallMaxCharge);
                currentCharge += Time.deltaTime * 10f;  
            }
           
        }

        if (currentCharge <= 0)
        {
            GameManager.instance.EndGame();
        }
    }

    public void AirDash()
    {
        airDashTimer = .2f;
        Debug.Log("In air dash");
        rb.velocity = new Vector2(0, 0);
        rb.gravityScale = 0f;
        if (facingRight)
        {
            rb.AddForce(new Vector2(1500f, 0f));
        }
        else
        {
            rb.AddForce(new Vector2(-1500f, 0f));
        }
        canAirDash = false;
    }

	private void OnTriggerEnter2D(Collider2D collision)
	
	{
        if (collision.gameObject.tag == "Upgrade")
        {
            Debug.Log(collision.gameObject);
            switch (collision.GetComponent<collectMe>().collectable)
            {
                case collectMe.CollectableType.airDash: 
                    GameManager.instance.airDashEnabled = true;
                    Destroy(collision.gameObject);
                    break;

                case collectMe.CollectableType.doubleJump:
                    GameManager.instance.doubleJumpEnabled = true;
                    Destroy(collision.gameObject);
                    break;

                case collectMe.CollectableType.wallJump:
                    GameManager.instance.wallJumpEnabled = true;
                    Destroy(collision.gameObject);
                    break;
            }
        }
	}

}
