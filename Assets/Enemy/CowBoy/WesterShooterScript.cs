using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WesterShooterScript : MonoBehaviour
{
    [SerializeField] private float speed = 2f;

    private Vector2 startPos;

    private bool lookRight = true;

    private SpriteRenderer sp;
    
    // variable pour le shoot
    [SerializeField] private float shootDistance = 5f;
    [SerializeField] private bool shoot = false;

    private Animator anim;

    private AudioSource audios;
    //layers pour eviter que le raycast touche le character de l'enemy
    [SerializeField] private LayerMask layer;
    
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audios = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!shoot)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            // on va utiliser Vector2.distance pour vérifier si la distance entre 2 vecteurs
            if (Vector2.Distance(transform.position, startPos) < 0.5f && !lookRight)
            {
                FlipCharacter();
            }
        }
        
        // IA shoot
        Vector2 rayDirection = lookRight ? Vector2.right : Vector2.left;
        // layer pour filtrer le raycast
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, shootDistance, layer);
        Debug.DrawRay(transform.position, rayDirection * shootDistance);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Chevalier"))
            {
                shoot = true;
                anim.SetBool("shoot", true);
                if (!audios.isPlaying)
                    audios.Play();
            } 
        }
        else
        {
            shoot = false;
            anim.SetBool("shoot", false);
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
