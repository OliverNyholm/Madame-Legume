using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {

    public float jumpHeight = 4;
    public float timeToJumpApex = .4f;


    //private float accelerationTimeAirborne = .2f;
    //private float accelerationTimeGrounded = .1f;
    private float moveSpeed = 8;

    private float gravity;
    private float jumpVelocity;
    private Vector3 velocity;
    private float velocityXSmoothing;

    private bool facingRight = true;

    Controller2D controller;

    Animator anim;
	// Use this for initialization
	void Start () {
        controller = GetComponent<Controller2D>();
        anim = GetComponent<Animator>();


        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        print("Gravity: " + gravity + "jumpVelocity: " + jumpVelocity);
	}

    void Update()
    {
        if(controller.collisions.above ||controller.collisions.below)
        {
            velocity.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        float move = Input.GetAxis("Horizontal");
        anim.SetFloat("Speed", Mathf.Abs(move));

        #region Jump
        if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below)
        {
            velocity.y = jumpVelocity;
        }
        #endregion

        

        float targetVelocityX = input.x * moveSpeed;
        //velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.x = targetVelocityX;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        #region Flip Player
        if (move > 0 && !facingRight) //flip player
            Flip();
        else if (move < 0 && facingRight)
            Flip();
        #endregion
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

}
