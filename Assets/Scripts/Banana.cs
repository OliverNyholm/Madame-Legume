using UnityEngine;
using System.Collections;

public class Banana : MonoBehaviour {

    [SerializeField]
    private float impulse;

    PlayerController controller;

    void OnCollisionStay2D(Collision2D col)
    {
        controller = col.gameObject.GetComponent<PlayerController>();

        controller = col.gameObject.GetComponent<PlayerController>();
        controller.onGround = false;

        Vector2 jumpForce = new Vector2(0, impulse);
        col.gameObject.GetComponent<Rigidbody2D>().AddForce(jumpForce, ForceMode2D.Impulse);

    }
}
