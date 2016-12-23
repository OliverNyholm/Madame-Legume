using UnityEngine;
using System.Collections;

public class MoveableVegetable : MonoBehaviour {

    Vector3 offset;

   void OnMouseDown()
    {
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
    }

    void OnMouseDrag()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        Vector3 newposition = Camera.main.ScreenToWorldPoint(mousePos) + offset;
        

        transform.position = newposition;
    }
}
