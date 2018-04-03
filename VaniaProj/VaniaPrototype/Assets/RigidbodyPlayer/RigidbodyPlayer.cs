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
    public bool facingRight;
    public float maxCharge, smallMaxCharge;
    public float currentCharge;
    public bool onPlatform, haveTouchedPlatform = false;
    public Image chargeImage;

    [Header("Can Use Specials")]
    public bool canDoubleJump = false;
    public bool canAirDash = false;

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
        GravityMultiplier();
       
    }
    private void Update()
    {
        //Debug.Log("velocity: " + rb.velocity);
        PlayerMovement();
        ChargeCheck();

        if (GameManager.instance.airDashEnabled)
        {
            if (canAirDash && Input.GetKey(myInputs[3]))
            {
                AirDash();
            }
        }
            PlayerJump();
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
            facingRight = false;
        }
        if (Input.GetKey(myInputs[1]))
        {
            transform.Translate(Vector2.right * (moveForce * Time.deltaTime));
            facingRight = true;
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
            if (CanJump)
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
        if (!onPlatform)
        {
            currentCharge -= Time.deltaTime * 12;
            GameManager.instance.currentCharge--;
            haveTouchedPlatform = false;
        }
        if (onPlatform)
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
        rb.velocity = new Vector2(0, 0);
        if (facingRight)
        {
            rb.AddForce(new Vector2(500f, 0f));
        }
        else
        {
            rb.AddForce(new Vector2(-500f, 0f));
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
