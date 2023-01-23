using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightScript : MonoBehaviour {

    [SerializeField] float speed = 2f, jumpForce = 500f;
    Rigidbody2D rb;
    Animator anim;
    bool lookRight = true;
    public bool OnAttack = false;
    public bool canJump = false;

    //grounded
    [SerializeField] bool grounded;
    [SerializeField] float groundRadius = 0.02f;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask theGround;

    [SerializeField] private AudioClip sndAttack, sndJump, sndHurt, sndDead, sndWin, sndGoblin;
    AudioSource audioS;
    
    [SerializeField]
    private ProgressBar pb;

    void Awake () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioS = GetComponent<AudioSource>();
	}	
	
	void Update () {

        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, theGround);
        anim.SetBool("Grounded", grounded);

        float move = Input.GetAxis("Horizontal");
        transform.Translate(Vector2.right * move * speed * Time.deltaTime);
        anim.SetFloat("Speed", Mathf.Abs(move));
        anim.SetFloat("Vspeed", rb.velocity.y);

        if(move>0 && !lookRight)
        {
            Flip();
        }
        else if(move < 0 && lookRight)
        {
            Flip();
        }

        if(Input.GetKeyDown(KeyCode.LeftAlt) && grounded && !OnAttack)
        {
            OnAttack = true;
            anim.SetTrigger("Attack");
            audioS.PlayOneShot(sndAttack);
        }

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            canJump = true;
        }

        ///Debug
        if (Input.GetKeyDown(KeyCode.F1)) Hurt();
        if (Input.GetKeyDown(KeyCode.F2)) Win();
        if (Input.GetKeyDown(KeyCode.F3)) Dead();

    }

    private void FixedUpdate()
    {
        if(canJump)
        {
            rb.AddForce(new Vector2(0, jumpForce));
            audioS.PlayOneShot(sndJump);
            canJump = false;
        }
    }


    void Flip()
    {
        lookRight = !lookRight;
        Vector2 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void Hurt()
    {
        anim.SetTrigger("Hurt");
        audioS.PlayOneShot(sndHurt);
        // decrementer la barre
        pb.Val -= 10;

        if (pb.Val <= 0)
        {
            Debug.Log("Game Over");
        }
    }

    public void Win()
    {
        anim.SetTrigger("Win");
        audioS.PlayOneShot(sndWin);
    }

    public void Dead()
    {
        anim.SetTrigger("Dead");
        audioS.PlayOneShot(sndDead);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "WesternShooter":
            case "Goblin":
                if (OnAttack)
                {
                    // on veut après avoir attaquer l'enemi on va pouvoir le faire sauter
                    Rigidbody2D rbGoblin = collision.gameObject.GetComponent<Rigidbody2D>();
                    // mais vu que notre rigidbody est mis en kinematic on va devoir le rendre dynamic
                    rbGoblin.bodyType = RigidbodyType2D.Dynamic;
                    rbGoblin.AddForce(Vector2.up * 2000);
                    audioS.PlayOneShot(sndGoblin);
                }
                else
                {
                    // on veut appliquer une force à notre personnage en fonction de la collision
                    PlayerHurtMove(collision);
                }
                break;
            case "Mace":
                PlayerHurtMove(collision);
                break;
        }
        
    }

    public void PlayerHurtMove(Collision2D collision)
    {
        Vector2 move =  collision.transform.position - transform.position;
        rb.AddForce(move.normalized * -200);
        Hurt();
    }

    public void AttackBool()
    {
        OnAttack = false;
    }
}
