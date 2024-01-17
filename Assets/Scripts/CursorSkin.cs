using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSkin : MonoBehaviour
{
    public Texture2D cursorHand;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 rayPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero);

        // Vérifier s'il y a une collision avec un objet
        if (hit.collider != null && hit.collider.gameObject.name != "IconeSouris" && !hit.collider.gameObject.CompareTag("Antivirus"))
        {
            Cursor.SetCursor(cursorHand, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
     }
}
