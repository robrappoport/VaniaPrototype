using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurroundingsCheck : MonoBehaviour {

    private Rigidbody2D rb;
    private RigidbodyPlayer player;
    public float raydist;
    public LayerMask myMask;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<RigidbodyPlayer>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, -Vector2.up, raydist, myMask);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, -Vector2.right, raydist, myMask);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, raydist, myMask);
        Debug.DrawRay(transform.position, -Vector2.up * raydist, Color.red);
        Debug.DrawRay(transform.position, -Vector2.right * raydist, Color.red);
        Debug.DrawRay(transform.position, Vector2.right * raydist, Color.red);

        if (hitDown.collider != null || hitLeft.collider != null || hitRight.collider != null)
        {
            Debug.Log(hitDown.collider);
            if (hitDown.collider != null)
            {
                if (hitDown.collider.tag == "Ground")
                {
                    player.CanJump = true;
                    player.onCharger = false;
                    player.canAirDash = true;
                }
                else if (hitDown.collider.tag == "Charger")
                {
                    player.onCharger = true;
                    player.CanJump = true;
                    player.canAirDash = true;
                }

            }

            if (hitLeft.collider != null)
            {
                if (hitLeft.collider.tag == "Ground" && GameManager.instance.wallJumpEnabled)
                {

                    player.CanJump = true;
                }
            }

            if (hitRight.collider != null)
            {
                if (hitRight.collider.tag == "Ground" && GameManager.instance.wallJumpEnabled)
                {

                    player.CanJump = true;
                }
            }

        }
        else
        {
           
                player.CanJump = false;
                player.onCharger = false;

        }

        if (!player.CanJump)
        {
            player.coyoteCounter -= Time.fixedDeltaTime;
        }
        else
        {
            player.coyoteCounter = .05f;
        }
    }
}
