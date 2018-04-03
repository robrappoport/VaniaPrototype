using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerTypeScript : MonoBehaviour {

    public bool bigCharger;
	// Use this for initialization
	void Start () {
		
	}


	private void OnCollisionEnter2D(Collision2D collision)
	{
        if (GameManager.instance.currentActiveCharger != this)
            if (collision.gameObject.tag == "Player")
            {
                GameManager.instance.AssignActiveCharger(this);
            }
        }
}


