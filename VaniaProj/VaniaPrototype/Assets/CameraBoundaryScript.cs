using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBoundaryScript : MonoBehaviour {

    public BoxCollider2D managerBox; //this is the collider of the BoundaryManager
    public Transform player; //this is the position of the player
    public GameObject boundary; //the camera boundary which will be activated/deactivated
	
	private void Start () {
        managerBox = GetComponent<BoxCollider2D>();
        player = Camera.main.GetComponent<CameraFollow>().target.transform;
	}
	
	// Update is called once per frame
	void Update () {
        ManageBoundary();
	}

    private void ManageBoundary()
    {
        if (managerBox.bounds.min.x < player.position.x && player.position.x < managerBox.bounds.max.x && 
            managerBox.bounds.min.y < player.position.y && player.position.y < managerBox.bounds.max.y)
        {
            boundary.SetActive(true);
        }
        else
        {
            boundary.SetActive(false);
        }
    }
}
