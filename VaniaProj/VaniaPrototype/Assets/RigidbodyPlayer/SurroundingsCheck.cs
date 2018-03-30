using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurroundingsCheck : MonoBehaviour {

    private Rigidbody2D rb;
    private RigidbodyPlayer player;
    public float raydist;
    [Header("Upgrades Enabled")]
    public bool doubleJumpEnabled = false;
    public bool wallJumpEnabled = false;
    public bool airDashEnabled = false;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<RigidbodyPlayer>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, -Vector2.up, raydist);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, -Vector2.right, raydist);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, raydist);
        Debug.DrawRay(transform.position, -Vector2.up * raydist, Color.red);
        Debug.DrawRay(transform.position, -Vector2.right * raydist, Color.red);
        Debug.DrawRay(transform.position, Vector2.right * raydist, Color.red);

        if (hitDown.collider != null || hitLeft.collider != null || hitRight.collider != null)
        {
            if (hitDown.collider != null)
            {
                if (hitDown.collider.tag == "Ground")
                {
                    player.CanJump = true;
                    player.onPlatform = false;
                }
                else if (hitDown.collider.tag == "Charger")
                {
                    player.onPlatform = true;
                    player.CanJump = true;
                }

            }

            if (hitLeft.collider != null)
            {
                if (hitLeft.collider.tag == "Ground")
                {

                    player.CanJump = true;
                }
            }

            if (hitRight.collider != null)
            {
                if (hitRight.collider.tag == "Ground")
                {

                    player.CanJump = true;
                }
            }

        }
        else
        {
           
                player.CanJump = false;
                player.onPlatform = false;

        }
    }
}
