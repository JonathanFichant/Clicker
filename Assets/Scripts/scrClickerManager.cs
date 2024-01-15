using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Burst.CompilerServices;

public class scrClickerManager : MonoBehaviour
{
    public ulong score = 0;
    public int forceMouse = 1;
    public int costForce = 10;
    public int costAutoMouse = 15;
    public TextMeshProUGUI scoreText;
    public bool stateAutoclick = false;
    public float cdAutoMouse = 1;
    public int costRange = 15;
    public float range = 5f; //13 max pour toucher le plus loin
    public float precision = 70;
    public int costPrecision = 15;
    public scrCanon scriptCanon;
    public SpriteRenderer spriteRendererAutoclick;
    public SpriteRenderer spriteRendererPrecision;
    private bool popUpActivate = false;
    public GameObject popupPrefab;
    public GameObject squareSelection;
    public AudioSource audioSource;
    public AudioClip soundAntivirus;
    public AudioClip soundClick;

    // rajouter une variable qui augmente l'augmentation du prix ?

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
            if (hit.collider != null && hit.collider.gameObject.name != "CircleSelection" && !hit.collider.gameObject.CompareTag("Antivirus"))
            {
                squareSelection.transform.position = hit.collider.transform.position;

                Animator animator = hit.collider.gameObject.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.Play("Bounce", 0, 0f);
                }
                
                if (hit.collider.gameObject.CompareTag("Target") && !(hit.collider.gameObject.name == "Pop-up"))
                {
                    Square(1);  // le clic simple n'est pas le but du concept ici donc il est plus faible et décourageant
                }
                else if (hit.collider.gameObject.name == "Upgrade")
                {
                    Upgrade();
                }
                else if (hit.collider.gameObject.name == "Autoclick" && score >= (ulong)costAutoMouse && cdAutoMouse > 0.1f)
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
                PlaySoundClick();
            }
            else
            {
                squareSelection.transform.position = new Vector2(-4, -8);
            }
        }
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
            if (score <= (ulong)force1)
            {
                score = 0;
            }
            else
            {
                score += (ulong)force;
            }
        }
        else
        {
            score += (ulong)force;
        }
        DisplayScore();

        if (!popUpActivate && score > 100)
        {
            popUpActivate = true;
            StartCoroutine(CoroutinePopUp());
        }
    }
    public IEnumerator CoroutinePopUp() // création de pop up régulièrement
    {
        while (true)
        {
            CreatePopup();
            yield return new WaitForSeconds(Random.Range(30,60));
        }
    }

    public void Upgrade()
    {
        if (score >= (ulong)costForce)
        {
            forceMouse++;
            score -= (ulong)costForce;
            costForce += 5;
            DisplayScore();
        }
    } // augmentation de la force des mouses

    public void Range()
    {
        if (score >= (ulong)costRange)
        {
            score -= (ulong)costRange;
            costRange += 5;
            range *= 1.007f;
            DisplayScore();
        }
    } // augmentation de la poussée initiale des mouses

    public void Precision()
    {
        if (score >= (ulong)costPrecision && precision > 0.3f)
        {
            score -= (ulong)costPrecision;
            costPrecision += 5;
            precision *= 0.95f;
            DisplayScore();
            if (precision <= 0.3f)
            {
                precision = 0f;
                spriteRendererPrecision.color = Color.gray;
            }
        }
    } // réduction de la marge aléatoire de l'angle de tir

    public void createMouse() // création auto de souris depuis le canon
    {
        float randomAngle = Random.Range(scriptCanon.finalAngle - precision, scriptCanon.finalAngle + precision);
        float angleInRadians = randomAngle * Mathf.Deg2Rad;
        float xx = Mathf.Cos(angleInRadians);
        float yy = Mathf.Sin(angleInRadians);
        scriptCanon.createMouse(new Vector2(xx, yy), range);
    }

    public void Autoclick()
    {
        score -= (ulong)costAutoMouse;
        costAutoMouse += 10;
        cdAutoMouse *= 0.95f;
        DisplayScore();
        if (cdAutoMouse <= 0.1f)
        {
            cdAutoMouse = 0.1f;
            spriteRendererAutoclick.color = Color.gray;
        }
        if (!stateAutoclick)
        {
            stateAutoclick = true;
            StartCoroutine(CoroutineAutoclick());
        }
    }

    public void DisplayScore() // Affichage du score, arrondi à l'entier
    {
        string scoreDisplay;
        if (score >= 137438953472) //128go
        {
            scoreDisplay = "100% !";
        }
        else
        {
            scoreDisplay = score.ToString("N0");
        }
        float precisionDisplay = precision * 2;
        scoreText.text =
            "Range : " + range.ToString() +
            "\nNext level : " + costRange.ToString("N0") + " o " +
            "\n\nPrecision : " + precisionDisplay.ToString("F2") + " degrés" +
            "\nNext level : " + costPrecision.ToString("N0") + " o " +
            "\n\nCooldown : " + cdAutoMouse.ToString("F2") + "s " +
            "\nNext level : " + costAutoMouse.ToString("N0") + " o " +
            "\n\nStrength : " + forceMouse.ToString("N0") +
            "\nNext level : " + costForce.ToString("N0") + " o " +
            "\n\nInfected bytes : " + scoreDisplay;
    }

    public void CreatePopup() // création de popup
    {
        float randomX = Random.Range(-4f, 4f);
        float randomY = Random.Range(0.5f, 3f);
        Instantiate(popupPrefab, new Vector2(randomX,randomY), transform.rotation);
        audioSource.volume = 0.7f;
        audioSource.PlayOneShot(soundAntivirus);
    }

    public void PlaySoundClick()
    {
        audioSource.volume = 0.2f;
        audioSource.PlayOneShot(soundClick);
    }
}