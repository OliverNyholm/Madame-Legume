using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float maxSpeed = 5;
    [SerializeField]
    private float jumpHeight = 4;
    [SerializeField]
    private float timeToJumpApex = .4f;

    private float gravity;
    private float jumpVelocity;
    private float velocityXSmoothing;

    private Rigidbody2D rigi;
    private Animator anim;

    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private LayerMask ground;
    private float groundCheckRadius = .3f;

    bool facingRight = true;
    public bool onGround = false;
    public bool onBanana = false;

    [SerializeField]
    private Object[] vegetable;
    private int vegetableIndex;

    VegetableOnMouse platformRender;
    private bool showPlatform;


    void Start()
    {
        rigi = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        platformRender = GetComponent<VegetableOnMouse>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // ----------- Move left and right -----------

        float move = Input.GetAxis("Horizontal");

        onGround = isOnGround();
        onBanana = isOnBanana();




        if (onBanana)
        {
            float targetVelocityX = move * maxSpeed;
            float temp = Mathf.SmoothDamp(rigi.velocity.x, targetVelocityX, ref velocityXSmoothing, 0.8f);
            rigi.velocity = new Vector2(temp, rigi.velocity.y);
        }
        else
            rigi.velocity = new Vector2(move * maxSpeed, rigi.velocity.y);

        anim.SetFloat("Speed", Mathf.Abs(move));

        // ----------- Flip the sprite depending on which way moving -----------
        if (move > 0 && !facingRight)
            Flip();
        else if (move < 0 && facingRight)
            Flip();
    }

    void Update()
    {
        if (onGround)
            rigi.velocity = new Vector2(rigi.velocity.x, 0);
        else
            rigi.velocity = new Vector2(rigi.velocity.x, rigi.velocity.y + gravity * Time.deltaTime);

        #region Jump
        if (Input.GetKeyDown(KeyCode.Space)) //Jump
        {
            if (onGround)
            {
                //
                rigi.velocity = new Vector2(rigi.velocity.x, jumpVelocity);
                onGround = false;
            }
        }
        #endregion

        #region Left Mouse
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!showPlatform)
            {
                showPlatform = true;
                platformRender.draw = true;
            }
            else
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = 2.0f;
                Vector3 objectPosition = Camera.main.ScreenToWorldPoint(mousePos);

                vegetable[vegetableIndex] = Instantiate(vegetable[vegetableIndex], objectPosition, Quaternion.identity);
            }
        }
        #endregion

        #region Right Mouse
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            showPlatform = false;
            platformRender.draw = false;
        }
        #endregion

        #region NumberClicks
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            vegetableIndex = 0;
            platformRender.index = vegetableIndex;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            vegetableIndex = 1;
            platformRender.index = vegetableIndex;
        }
        #endregion
    }

    // ----------- Check if on ground -----------
    private bool isOnGround()
    {
        if (rigi.velocity.y <= 0)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, ground);

            for (int i = 0; i < colliders.Length; ++i)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    return true;
                }
            }
        }
        return false;
    }

    // ----------- Check if on banana. Could be merged into isOnGround() -----------
    private bool isOnBanana()
    {
        if (rigi.velocity.y <= 0)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, ground);

            for (int i = 0; i < colliders.Length; ++i)
            {
                if (colliders[i].gameObject != gameObject && colliders[i].gameObject.layer == 11) //11 = Banana layer
                {
                    return true;
                }
            }
        }
        return false;
    }

    // ----------- Flip sprite -----------
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
