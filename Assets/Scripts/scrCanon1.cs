using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class scrCanon1 : MonoBehaviour
{
    public GameObject mousePrefab;
    public float baseAngle;
    public bool selected = false;
    public GameObject circleSelection;
    private GameObject circleInstance;
    public float radius = 3f;
    private bool circleDrag = false;
    private Vector2 dragOffset;
    public float angleCircle;
    public float finalAngle;
    public scrClickerManager scriptClickerManager;

    void Start()
    {
        baseAngle = 90f;
        finalAngle = baseAngle;
        CreateCircle();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            CheckClickOnCircle();
        }
        else circleDrag = false;

        if (circleDrag)
        {
            MoveCercleWithMouse();
        }
        if (selected)
        {
            circleInstance.SetActive(true);
        } // affichage cercle quand le canon est sélectionné
        else
        {
            circleInstance.SetActive(false);
        }

    }

    public void OnMouseDown() // sélection du canon, affichage cercle, création de souris
    {
        
        // déselectionner tous les autres canons
        // condition pour éviter clic trop rapproché
        float randomAngle = Random.Range(finalAngle - scriptClickerManager.precision, finalAngle + scriptClickerManager.precision);
        float angleInRadians = randomAngle * Mathf.Deg2Rad;
        float xx = Mathf.Cos(angleInRadians);
        float yy = Mathf.Sin(angleInRadians);;
        createMouse(new Vector2(xx,yy),scriptClickerManager.range);
      
    }

    public void createMouse(Vector2 direction, float speed)
    {
        GameObject mouse = Instantiate(mousePrefab, transform.position, transform.rotation);
        scrMouse mouseScript = mouse.GetComponent<scrMouse>();
        mouseScript.SetDirectionAndSpeed(direction, speed);
    } // génération de souris

    void CreateCircle()
    {
        circleInstance = Instantiate(circleSelection, GetCirclePosition(), transform.rotation);
        circleInstance.SetActive(false);   

    } // créé le cercle de sélection d'angle au start
    private Vector2 GetCirclePosition() // détermine la position du cercle au start
    {

        float angleInRadians = baseAngle * Mathf.Deg2Rad;

        // Calculer la position du cercle en fonction de l'angle et du rayon
        float x = transform.position.x + radius * Mathf.Cos(angleInRadians);
        float y = transform.position.y + radius * Mathf.Sin(angleInRadians);

        return new Vector2(x, y);
    }

    private void CheckClickOnCircle()
    {
        Vector2 rayPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject == circleInstance)
        {
            circleDrag = true;
            dragOffset = (Vector2)circleInstance.transform.position - rayPos;
        }
    } // détection du clic sur le cercle

    public void MoveCercleWithMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculer la nouvelle position du cercle en maintenant la distance fixe
        Vector2 newPosition = (Vector2)mousePosition + dragOffset;

        // Limiter la position à une distance fixe du canon
        Vector2 directionToCanon = newPosition - (Vector2)transform.position;
        newPosition = (Vector2)transform.position + directionToCanon.normalized * radius;

        // Appliquer la nouvelle position au cercle
        circleInstance.transform.position = newPosition;

        // Calculer l'angle en radians en fonction de la nouvelle position du cercle
        float angleInRadians = Mathf.Atan2(directionToCanon.y, directionToCanon.x);

        // Convertir l'angle en degrés
        float angleInDegrees = angleInRadians * Mathf.Rad2Deg;

        // Appliquer la rotation au canon en fonction de l'angle calculé
        transform.rotation = Quaternion.Euler(0, 0, angleInDegrees-90);
        finalAngle = angleInDegrees;
    }
}
