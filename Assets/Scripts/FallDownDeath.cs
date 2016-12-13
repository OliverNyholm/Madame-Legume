using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FallDownDeath : MonoBehaviour {

    //PlayerController controller;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "Player")
        {            
            Application.LoadLevel(Application.loadedLevel);
        }

    }
}
