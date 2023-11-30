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
    public bool StateAutoclick = false;
    public float cdAutoclick = 5;
    public int forceAutoclick = 1;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 rayPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero);

            // Vérifier s'il y a une collision avec un objet
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.name == "Square")
                {
                    score1 += score1Force;
                    
                }
                else if (hit.collider.gameObject.name == "Upgrade" && score1 >= costForce)
                {
                    score1Force += 0.1f;
                    score1 -= costForce;
                    costForce += 1;
                }
                else if (hit.collider.gameObject.name == "Autoclick" && score1 >= costAutoclick)
                {
                    score1 -= costAutoclick;
                    cdAutoclick *= 0.95f;
                    // réduction délai coroutine ici
                    if (!StateAutoclick)
                    {
                        StateAutoclick = true;
                        StartCoroutine(CoroutineAutoclick());
                    }
                    
                    
                }
                score1Text.text = score1.ToString();
            }




        }
    }
    public IEnumerator CoroutineAutoclick()
    {
        while (true)
        {
            score1 += forceAutoclick;
            score1Text.text = score1.ToString();
            yield return new WaitForSeconds(cdAutoclick);
        }
        
        
    }
}
