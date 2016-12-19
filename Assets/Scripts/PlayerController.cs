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
    private LayerMask ground;
    [SerializeField]
    private LayerMask banana;

    bool facingRight = true;
    public bool onGround = false;
    public bool onBanana = false;
    public bool leftBanana = false;
    private float bananaTimer = 0.3f;
    public bool hitTomato = false;
    private float tomatoTimer = 0.2f;

    [SerializeField]
    private Object[] vegetable;
    private int vegetableIndex;
    public Color vegetableColor;
    public int[] vegetableCount;

    VegetableOnMouse platformRender;
    private bool showPlatform;


    void Start()
    {
        rigi = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        platformRender = GetComponent<VegetableOnMouse>();
        vegetableCount = new int[3];

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

        // ----------- Timers for impulse speed on banana and tomato -----------
        if (leftBanana)
        {
            bananaTimer -= Time.deltaTime;

            if (bananaTimer < 0)
            {
                leftBanana = false;
                bananaTimer = 0.3f;
            }
        }

        if (hitTomato)
        {
            tomatoTimer -= Time.deltaTime;

            if (tomatoTimer < 0)
            {
                hitTomato = false;
                tomatoTimer = 0.2f;
            }
        }

        // ----------- Delay movement depending if on banana or tomato -----------
        if (onBanana || leftBanana)
        {
            float targetVelocityX = move * maxSpeed;
            float temp = Mathf.SmoothDamp(rigi.velocity.x, targetVelocityX, ref velocityXSmoothing, 0.8f);
            rigi.velocity = new Vector2(temp, rigi.velocity.y);
        }
        else if(hitTomato)
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
                //showPlatform = true;
                //platformRender.draw = true;
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
            DrawVegetables();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            vegetableIndex = 1;
            DrawVegetables();
        }
        #endregion
    }

    // ----------- Check if on ground -----------
    private bool isOnGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.2f, ground);

        Debug.DrawRay(transform.position, Vector2.down, Color.red);

        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }

    // ----------- Check if on banana. Could be merged into isOnGround() -----------
    private bool isOnBanana()
    {
        RaycastHit2D hitCenter = Physics2D.Raycast(transform.position, Vector2.down, 1.2f, banana);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position - new Vector3(.3f, 0, 0), Vector2.down, 1.2f, banana);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position + new Vector3(.3f, 0, 0), Vector2.down, 1.2f, banana);

        Debug.DrawRay(transform.position, Vector2.down, Color.green);
        Debug.DrawRay(transform.position - new Vector3(.3f, 0, 0), Vector2.down, Color.green);
        Debug.DrawRay(transform.position + new Vector3(.3f, 0, 0), Vector2.down, Color.green);

        if (hitCenter.collider != null || hitLeft.collider != null || hitRight.collider != null)
        {
            return true;
        }
        else
        {
            if (onBanana)
                leftBanana = true;
            return false;
        }
    }

    // ----------- Flip sprite -----------
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void DrawVegetables()
    {
        platformRender.index = vegetableIndex;
        showPlatform = true;
        platformRender.draw = true;
        vegetableColor = new Color(255, 255, 255, 100);

        if (vegetableCount[vegetableIndex] == 0)
            vegetableColor = new Color(255, 0, 0, 100);
    }
}
