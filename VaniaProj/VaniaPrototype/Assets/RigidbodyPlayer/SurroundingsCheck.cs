using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurroundingsCheck : MonoBehaviour {

    private Rigidbody2D rb;
    public float raydist;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
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
                    GetComponent<RigidbodyPlayer>().CanJump = true;
                    GetComponent<RigidbodyPlayer>().onPlatform = false;
                }
                else if (hitDown.collider.tag == "Charger")
                {
                    GetComponent<RigidbodyPlayer>().onPlatform = true;
                    GetComponent<RigidbodyPlayer>().CanJump = true;
                }

            }

            if (hitLeft.collider != null)
            {
                if (hitLeft.collider.tag == "Ground")
                {

                    GetComponent<RigidbodyPlayer>().CanJump = true;
                }
            }

            if (hitRight.collider != null)
            {
                if (hitRight.collider.tag == "Ground")
                {

                    GetComponent<RigidbodyPlayer>().CanJump = true;
                }
            }

        }
        else
        {
            GetComponent<RigidbodyPlayer>().CanJump = false;
            GetComponent<RigidbodyPlayer>().onPlatform = false;
        }
    }
}
