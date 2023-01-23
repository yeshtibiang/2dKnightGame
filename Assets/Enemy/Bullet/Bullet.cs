using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // detruisons le game object après 4 secondes
        Destroy(gameObject, 4f);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // appeler hurt du player
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<KnightScript>().Hurt();
        }
        Destroy(gameObject);
    }
}
