using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FallDownDeath : MonoBehaviour {

    PlayerController player;
    Vector2 startPosition;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        startPosition = player.transform.position;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "Player")
        {
            //SceneManager.LoadScene(nextScene);
            player.transform.position = startPosition;
        }
    }
}
