using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    public BoxCollider2D colliderPopUp;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // zone de 0.2f sur 0.2f
        {
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (clickPosition.x >= (transform.position.x + colliderPopUp.size.x / 2f) -0.2f && //
                clickPosition.y >= (transform.position.y + colliderPopUp.size.y / 2f) - 0.2f &&
                clickPosition.x <= (transform.position.x + colliderPopUp.size.x / 2f) &&
                 clickPosition.y <= (transform.position.y + colliderPopUp.size.y / 2f))
            {
                Destroy(gameObject);
            }
        } // v�rif si clic sur le coin du pop up
    }
}
