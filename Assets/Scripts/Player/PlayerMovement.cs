using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime; //How much time the player can hang in the air before jumping
    private float coyoteCounter; //How much time passed since the player ran off the edge

    [Header("Wall Jumping")]
    [SerializeField] private float wallJumpX; //Horizontal wall jump force
    [SerializeField] private float wallJumpY; //Vertical wall jump force

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header("Sounds")]
    [SerializeField] private AudioClip jumpSound;

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float horizontalInput;

    private void Awake()
    {
        //Grab references for rigidbody and animator from object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        //Flip player when moving left-right
        if (GetComponent<Health>().currentHealth > 0)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            if (horizontalInput > 0.01f)
                transform.localScale = new Vector3(0.5f, 0.5f, 1);
            else if (horizontalInput < -0.01f)
                transform.localScale = new Vector3(-0.5f, 0.5f, 1);
            //Set animator parameters
            anim.SetBool("Run", horizontalInput != 0);
            anim.SetBool("Grounded", isGrounded());
        }


        //Jump
        if (Input.GetKeyDown(KeyCode.W) && GetComponent<Health>().currentHealth > 0)
            Jump();

        //Adjustable jump height
        if (Input.GetKeyUp(KeyCode.W) && body.velocity.y > 0)
            body.velocity = new Vector2(body.velocity.x, body.velocity.y * 0.33f);

        if (onWall())
        {
            body.gravityScale = 0;
            body.velocity = Vector2.zero;
        }
        else
        {
            body.gravityScale = 7;
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (isGrounded())
            {
                coyoteCounter = coyoteTime; //Reset coyote counter when on the ground
            }
            else
                coyoteCounter -= Time.deltaTime; //Start decreasing coyote counter when not on the ground
        }
    }

    private void Jump()
    {
        if (coyoteCounter <= 0 && !onWall()) return;
        //If coyote counter is 0 or less and not on the wall 

        SoundManager.instance.PlaySound(jumpSound);

        if (onWall())
            WallJump();
        else
        {
            if (isGrounded())
                body.velocity = new Vector2(body.velocity.x, jumpPower);
            
            //Reset coyote counter to 0 to avoid double jumps
            coyoteCounter = 0;
        }
    }

    private void WallJump()
    {
        body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) 
            * wallJumpX, wallJumpY));
    }


    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, 
            boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, 
            boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
    public bool canAttack()
    {
        return horizontalInput == 0 && !onWall();
    }
}