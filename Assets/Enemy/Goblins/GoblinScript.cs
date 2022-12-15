using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinScript : MonoBehaviour
{
    [SerializeField] private float speed = 2f;

    private Vector2 startPos;

    private bool lookRight = true;

    private SpriteRenderer sp;

    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        // on va utiliser Vector2.distance pour vérifier si la distance entre 2 vecteurs
        if (Vector2.Distance(transform.position, startPos) < 0.5f && !lookRight)
        {
            FlipCharacter();
        }   
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "PointRetour")
        {
            FlipCharacter();
            // on veut que le goblin puisse retourner à sa position d'origine et ensuite repart vers pointRetour
            
        }
    }
    
    // flip character 
    void FlipCharacter()
    {
        sp.flipX = !sp.flipX;
        // on inverse le speed pour que notre charactère ne puisse pas continuer d'aller à droite
        speed = -speed;
        lookRight = !lookRight;
    }
}
