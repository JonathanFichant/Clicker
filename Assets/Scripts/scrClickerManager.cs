using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scrClickerManager : MonoBehaviour
{
    public ulong score1 = 0;
    public int forceMouse = 1;
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
    public SpriteRenderer spriteRendererAutoclick;
    public SpriteRenderer spriteRendererPrecision;
    // rajouter une variable qui augmante l'augmentation du prix ?

    void Start()
    {
        precision = 60f;
        range = 5f;
        forceMouse = 1;
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
                if (hit.collider.gameObject.CompareTag("Target") && !(hit.collider.gameObject.name == "Pop-up"))
                {
                    Square(1);  // le clic simple n'est pas le but du concept ici donc il est plus faible et décourageant
                    
                }
                else if (hit.collider.gameObject.name == "Upgrade")
                {
                    Upgrade();
                }
                else if (hit.collider.gameObject.name == "Autoclick" && score1 >= (ulong)costAutoMouse && cdAutoMouse > 0.1f)
                {
                    Autoclick();
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
            createMouse();
            yield return new WaitForSeconds(cdAutoMouse);
        }        
    }

    public void Square(int force) // augmentation du score
    {
        if (force < 0)
        {
            int force1 = -force;
            if (score1 <= (ulong)force1)
            {
                score1 = 0;
            }
            else
            {
                score1 += (ulong)force;
            }
        }
        else
        {
            score1 += (ulong)force;
        }
        DisplayScore();

    }

    public void Upgrade()
    {
        if (score1 >= (ulong)costForce)
        {
            forceMouse ++;
            score1 -= (ulong)costForce;
            costForce += 5;
            DisplayScore();
        }
    } // augmentation de la force des mouses

    public void Range()
    {
        if (score1 >= (ulong)costRange)
        {
            score1 -= (ulong)costRange;
            costRange += 5;
            range *= 1.005f;
            DisplayScore();
        }
    } // augmentation de la poussée initiale des mouses

    public void Precision()
    {
        if (score1 >= (ulong)costPrecision && precision > 0.3f)
        {
            score1 -= (ulong)costPrecision;
            costPrecision += 5;
            precision *= 0.97f;
            DisplayScore();
            if (precision <= 0.3f)
            {
                precision = 0f;
                spriteRendererPrecision.color = Color.gray;
            }
        }
    } // réduction de la marge aléatoire de l'angle de tir

    public void createMouse() // création auto de souris depuis le canon principal
    {
        float randomAngle = Random.Range(scriptCanon1.finalAngle - precision, scriptCanon1.finalAngle + precision);
        float angleInRadians = randomAngle * Mathf.Deg2Rad;
        float xx = Mathf.Cos(angleInRadians);
        float yy = Mathf.Sin(angleInRadians);
        scriptCanon1.createMouse(new Vector2(xx, yy), range);
    }

    public void Autoclick()
    {
        score1 -= (ulong)costAutoMouse;
        costAutoMouse += 10;
        DisplayScore();
        cdAutoMouse *= 0.95f;
        if (cdAutoMouse <= 0.1f)
        {
            cdAutoMouse = 0.1f;
            spriteRendererAutoclick.color = Color.gray;
        }
        if (!StateAutoclick)
        {
            StateAutoclick = true;
            StartCoroutine(CoroutineAutoclick());
        }
    }

    public void DisplayScore() // Affichage du score, arrondi à l'entier
    {
        string score1Display;
        if (score1 >= 137438953472)
        {
            score1Display = "100% !";
        } // 128 Go
        else
        {
            score1Display = score1.ToString("N0");
        }
        float precisionDisplay = precision * 2;
        score1Text.text =
            "Range : " + range.ToString() +
            "\nNext level : " + costRange.ToString("N0") + " o " +
            "\n\nPrecision : " + precisionDisplay.ToString("F2") + " degrés" +
            "\nNext level : " + costPrecision.ToString("N0") + " o " +
            "\n\nCooldown : " + cdAutoMouse.ToString("F2") + "s " +
            "\nNext level : " + costAutoMouse.ToString("N0") + " o " +
            "\n\nStrength : " + forceMouse.ToString("N0") +
            "\nNext level : " + costForce.ToString("N0") + " o " +
            "\n\nInfected bytes : " + score1Display;
    }

}
