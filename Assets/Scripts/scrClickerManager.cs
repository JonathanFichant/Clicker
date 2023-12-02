using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scrClickerManager : MonoBehaviour
{
    public float score1 = 0;
    public float forceMouse = 1;
    public int costForce = 10;
    public int costAutoMouse = 15;
    public TextMeshProUGUI score1Text;
    public bool StateAutoclick = false;
    public float cdAutoMouse = 1;
    public scrCanon1 scriptCanon1;

    void Start()
    {

    }

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
                    Square(1);  // le clic simple n'est pas le but du concept ici donc il est plus faible et décourageant
                }
                else if (hit.collider.gameObject.name == "Upgrade")
                {
                    Upgrade();
                }
                else if (hit.collider.gameObject.name == "Autoclick" && score1 >= costAutoMouse && cdAutoMouse > 0.1f)
                {
                    score1 -= costAutoMouse;
                    DisplayScore();
                    cdAutoMouse *= 0.95f;
                    if (cdAutoMouse <= 0.1f)
                    {
                        cdAutoMouse = 0.1f;
                        //afficher MAX et griser le bouton
                    }
                    if (!StateAutoclick)
                    {
                        StateAutoclick = true;
                        StartCoroutine(CoroutineAutoclick());
                    }
                }
            }
        } // détection du bouton cliqué
    }
    public IEnumerator CoroutineAutoclick() // création de mouse automatique selon un cooldown
    {
        while (true)
        {
            Autoclick();
            yield return new WaitForSeconds(cdAutoMouse);
        }        
    }

    public void Square(float force) // augmentation du score
    {
        score1 += force;
        DisplayScore();
    }

    public void Upgrade()
    {
        if (score1 >= costForce)
        {
            forceMouse += 1f;
            score1 -= costForce;
            costForce += 5;
            DisplayScore();
        }
    } // augmentation de la force des mouses

    public void Autoclick() // création auto de souris depuis le canon principal
    {
        float randomAngle = Random.Range(scriptCanon1.finalAngle - 4f, scriptCanon1.finalAngle + 4f);
        float angleInRadians = randomAngle * Mathf.Deg2Rad;
        float xx = Mathf.Cos(angleInRadians);
        float yy = Mathf.Sin(angleInRadians);
        scriptCanon1.createMouse(new Vector2(xx, yy), scriptCanon1.forceCanon1);
    }

    public void DisplayScore() // Affichage du score, arrondi à l'entier
    {
        int roundScore = (int)score1;
        score1Text.text = roundScore.ToString("N0");
    }

}
