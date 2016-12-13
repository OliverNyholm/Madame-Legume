using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VegetableOnMouse : MonoBehaviour
{

    //[SerializeField]
    //SpriteRenderer[] images;

    [SerializeField]
    Sprite[] images;

    //[SerializeField]
    private GameObject vegetableImage;
    private Image test;

    public bool draw = false;
    public int index;

    // Use this for initialization
    void Start()
    {
        index = 0;
        vegetableImage = GameObject.Find("Vegetable");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 2f;


        if (draw)
        {
            vegetableImage.SetActive(true);
            vegetableImage.transform.position = mousePos;

            // ----------- Set HUD image to the size of the real image -----------
            var vegetableImageTransform = vegetableImage.transform as RectTransform;
            vegetableImageTransform.sizeDelta = new Vector2(images[index].textureRect.width, images[index].textureRect.height);

            GameObject.Find("Vegetable").GetComponent<Image>().sprite = images[index];            
        }
        else
        {
            vegetableImage.SetActive(false);
        }
    }
}
