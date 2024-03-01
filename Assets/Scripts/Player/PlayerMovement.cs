using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    private Animator anim;
    private bool grounded;
    [SerializeField] private AudioClip jumpSound;
    [Header ("Multiple Jumps")]
    [SerializeField] private int jumpSpeed;
    private int jumpCounter;

    [Header ("Coyote Time")]
    [SerializeField] private float coyoteTime;
    private float coyoteCounter;
    private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header("Wall Jumping")]
    [SerializeField] private float wallJumpX; //Horizontal wall jump force
    [SerializeField] private float wallJumpY; //Vertical wall jump force
    private float wallJumpCooldown;


    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake() {
        //Grab references
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    void Update()
    {

        if(isGrounded()) {

            
        }
        float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed * 1.5f, body.velocity.y);

        //Flip player when moving 
        if(horizontalInput > 0.01f) {
            transform.localScale = new Vector3(10, 10, 10);
        } else if (horizontalInput < -0.01f) {
            transform.localScale = new Vector3(-10, 10, 10);
        }

        if(Input.GetKeyDown(KeyCode.UpArrow) && grounded) {
            Jump();

        }

        //Adjustable jump height
        if(Input.GetKeyUp(KeyCode.UpArrow) && body.velocity.y > 0) {
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);

        }

        if (onWall()) {
            body.gravityScale = 0;
            body.velocity =  Vector2.zero;
        } else {
            body.gravityScale = 7;
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if(isGrounded()) {
                coyoteCounter = coyoteTime;

            } else {
                coyoteCounter -= Time.deltaTime;
            }
        }

        //Set animator parametrs
        anim.SetBool("Run", horizontalInput != 0);
        anim.SetBool("grounded", grounded);
        
    }





    private void Jump() {
        if(coyoteCounter <= 0 && !onWall()) {
            return;
        }
        SoundManager.instance.PlaySound(jumpSound);

        if(onWall()) {
            WallJump();
        } else {
            if(isGrounded()) {
                body.velocity = new Vector2(body.velocity.x, jumpPower);
            }
            coyoteCounter = 0;
        }
        
        body.velocity = new Vector2(body.velocity.x, jumpSpeed);
        anim.SetTrigger("jump");
        grounded = false;
    }

     private void WallJump()
    {
        body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
        wallJumpCooldown = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            grounded = true;
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
    

    /*public bool canAttack() {
        return horizontalInput == 0

    }
    */
    
    // Update is called once per frame
    
}
