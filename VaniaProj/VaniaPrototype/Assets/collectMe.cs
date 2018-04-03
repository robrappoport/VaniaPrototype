using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectMe : MonoBehaviour {

    public CollectableType collectable;

	public enum CollectableType
    {
        doubleJump, wallJump, airDash
    }

    public void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
