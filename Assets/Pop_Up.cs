using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pop_Up : MonoBehaviour
{
    public BoxCollider2D colliderPopUp;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // zone de 0.4f sur 0.4f
        {
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (clickPosition.x >= (transform.position.x + colliderPopUp.size.x / 2f) -0.2f && //
                clickPosition.y >= (transform.position.y + colliderPopUp.size.y / 2f) - 0.2f &&
                clickPosition.x <= (transform.position.x + colliderPopUp.size.x / 2f) &&
                 clickPosition.y <= (transform.position.y + colliderPopUp.size.y / 2f))
            {
                Destroy(gameObject);
            }
        }


    }
}
