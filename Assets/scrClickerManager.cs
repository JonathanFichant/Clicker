using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scrClickerManager : MonoBehaviour
{
    public float score1 = 0;
    public float score1Force = 1;
    public int costForce = 10;
    public int costAutoclick = 15;
    public TextMeshProUGUI score1Text;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Convertir la position de la souris en un rayon dans l'espace 2D
            Vector2 rayPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero);

            // Vérifier s'il y a une collision avec un objet
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.name == "Square")
                {
                    score1 += score1Force;
                    score1Text.text = score1.ToString();
                }
                else if (hit.collider.gameObject.name == "Upgrade" && score1 >= costForce)
                {
                    score1Force += 0.1f;
                    score1 -= costForce;
                    costForce += 1;
                    score1Text.text = score1.ToString();
                }
                else if (hit.collider.gameObject.name == "Autoclick" && score1 >= costAutoclick)
                {
                    // coroutine ici
                }
            }




        }
    }
}
