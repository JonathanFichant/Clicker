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
    public int costRange = 15;
    public float range = 5f; //13 max pour toucher le plus loin
    public float precision = 70;
    public int costPrecision = 15;
    public scrCanon1 scriptCanon1;

    // rajouter une variable qui augmante l'augmentation du prix

    void Start()
    {
        precision = 70f;
        range = 5f;
        forceMouse = 1f;
        cdAutoMouse = 2f;
        DisplayScore();
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
                if (hit.collider.gameObject.CompareTag("Target"))
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
                    costAutoMouse += 15;
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
                else if (hit.collider.gameObject.name == "Precision")
                {
                    Precision();                  
                }
                else if (hit.collider.gameObject.name == "Range")
                {
                    Range();
                }
                else if (hit.collider.gameObject.name == "Canon1")
                {
                    scriptCanon1.selected = true;
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

    public void Range()
    {
        if (score1 >= costRange)
        {
            
            score1 -= costRange;
            costRange += 5;
            range *= 1.005f;
            DisplayScore();
        }
    } // augmentation de la poussée initiale des mouses

    public void Precision()
    {
        if (score1 >= costPrecision)
        {
            score1 -= costPrecision;
            costPrecision += 5;
            DisplayScore();
            precision *= 0.97f;
        }
    } // réduction de la marge aléatoire de l'angle de tir

    public void Autoclick() // création auto de souris depuis le canon principal
    {
        float randomAngle = Random.Range(scriptCanon1.finalAngle - precision, scriptCanon1.finalAngle + precision);
        float angleInRadians = randomAngle * Mathf.Deg2Rad;
        float xx = Mathf.Cos(angleInRadians);
        float yy = Mathf.Sin(angleInRadians);
        scriptCanon1.createMouse(new Vector2(xx, yy), range);
    }

    public void DisplayScore() // Affichage du score, arrondi à l'entier
    {
        int roundScore = (int)score1;
        //score1Text.text = "Octets infectés : " + roundScore.ToString("N0");
        score1Text.text =
            "Range : " + range.ToString() +
            "\nNext level : " + costRange.ToString("N0") + " o " +
            "\n\nPrecision : " + precision.ToString() +
            "\nNext level : " + costPrecision.ToString("N0") + " o " +
            "\n\nCooldown : " + cdAutoMouse.ToString() + "s " +
            "\nNext level : " + costAutoMouse.ToString("N0") + " o " +
            "\n\nStrength : " + forceMouse.ToString() +
            "\nNext level : " + costForce.ToString("N0") + " o " +
            "\n\nInfected bytes : " + roundScore.ToString("N0");
    }

}
