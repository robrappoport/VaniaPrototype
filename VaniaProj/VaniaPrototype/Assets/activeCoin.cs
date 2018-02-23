using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activeCoin : MonoBehaviour {
    public float lifeTime;
    private void Awake()
    {
        lifeTime = Random.Range(3f, 5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.instance.score += 500;
            GameManager.instance.coinExists = false;
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0f)
        {
            GameManager.instance.coinExists = false;
            Destroy(gameObject);
        }
    }
}
