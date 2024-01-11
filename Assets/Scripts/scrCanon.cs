using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class scrCanon : MonoBehaviour
{
    public GameObject mousePrefab;
    public float baseAngle;
    public GameObject circleSelection;
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
    }

    public void OnMouseDown() // création de souris
    {
        // condition pour éviter clic trop rapproché ?
        float randomAngle = Random.Range(finalAngle - scriptClickerManager.precision, finalAngle + scriptClickerManager.precision);
        float angleInRadians = randomAngle * Mathf.Deg2Rad;
        float xx = Mathf.Cos(angleInRadians);
        float yy = Mathf.Sin(angleInRadians);;
        createMouse(new Vector2(xx,yy),scriptClickerManager.range);
    }

    public void createMouse(Vector2 direction, float speed) // génération de souris
    {
        GameObject mouse = Instantiate(mousePrefab, transform.position, transform.rotation);
        scrMouse mouseScript = mouse.GetComponent<scrMouse>();
        mouseScript.SetDirectionAndSpeed(direction, speed);
    }

    void CreateCircle() // création du cercle de sélection d'angle
    {
        //circleInstance = Instantiate(circleSelection, GetCirclePosition(), transform.rotation);
        circleSelection.transform.position = GetCirclePosition();
    }

    private Vector2 GetCirclePosition() // détermine la position du cercle au start
    {
        float angleInRadians = baseAngle * Mathf.Deg2Rad;
        float x = transform.position.x + radius * Mathf.Cos(angleInRadians); // Calculer la position du cercle en fonction de l'angle et du rayon
        float y = transform.position.y + radius * Mathf.Sin(angleInRadians);
        return new Vector2(x, y);
    }

    private void CheckClickOnCircle() // détection du clic sur le cercle
    {
        Vector2 rayPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject == circleSelection)
        {
            circleDrag = true;
            dragOffset = (Vector2)circleSelection.transform.position - rayPos;
        }
    }

    public void MoveCercleWithMouse() 
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculer la nouvelle position du cercle en maintenant la distance fixe
        Vector2 newPosition = (Vector2)mousePosition + dragOffset;

        // Limiter la position à une distance fixe du canon
        Vector2 directionToCanon = newPosition - (Vector2)transform.position;
        newPosition = (Vector2)transform.position + directionToCanon.normalized * radius;

        circleSelection.transform.position = newPosition;  // Appliquer la nouvelle position au cercle

        // Calculer l'angle en radians en fonction de la nouvelle position du cercle
        float angleInRadians = Mathf.Atan2(directionToCanon.y, directionToCanon.x);

        // Convertir l'angle en degrés
        float angleInDegrees = angleInRadians * Mathf.Rad2Deg;

        // Appliquer la rotation au canon en fonction de l'angle calculé
        transform.rotation = Quaternion.Euler(0, 0, angleInDegrees-90);
        finalAngle = angleInDegrees;
    }
}
