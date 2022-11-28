using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightScript : MonoBehaviour {

    [SerializeField] float speed = 2f, jumpForce = 500f;
    Rigidbody2D rb;
    Animator anim;
    bool lookRight = true;
    public bool OnAttack = false;

    //grounded
    [SerializeField] bool grounded;
    [SerializeField] float groundRadius = 0.02f;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask theGround;

    [SerializeField] AudioClip sndAttack, sndJump, sndHurt, sndDead, sndWin;
    AudioSource audioS;

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

        ///Debug
        if (Input.GetKeyDown(KeyCode.F1)) Hurt();
        if (Input.GetKeyDown(KeyCode.F2)) Win();
        if (Input.GetKeyDown(KeyCode.F3)) Dead();

    }

    private void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(new Vector2(0, jumpForce));
            audioS.PlayOneShot(sndJump);
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
        if(collision.gameObject.CompareTag("Chevalier"))
        {
            if(OnAttack)
            {
                Destroy(collision.gameObject);
            }
            else
            {
                Hurt();
            }
        }
    }

    public void AttackBool()
    {
        OnAttack = false;
    }
}
