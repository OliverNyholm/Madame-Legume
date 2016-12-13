using UnityEngine;
using System.Collections;

public class Tomato : MonoBehaviour
{

    [SerializeField]
    private float impulse;

    PlayerController controller;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "Player")
        {
            controller = col.gameObject.GetComponent<PlayerController>();
            controller.onGround = false;

            Vector2 jumpForce = new Vector2(0, impulse);
            col.gameObject.GetComponent<Rigidbody2D>().AddForce(jumpForce, ForceMode2D.Impulse);
        }

    }
}
