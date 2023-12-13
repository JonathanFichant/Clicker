using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class scrMouse : MonoBehaviour
{
    public Rigidbody2D rbMouse;
    public scrClickerManager scriptClickerManager;

    void Start()
    {
        scriptClickerManager = FindObjectOfType<scrClickerManager>();
    }

    void Update()
    {
        if (rbMouse.velocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(rbMouse.velocity.y, rbMouse.velocity.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, angle - 90f);
        }

        if (transform.position.y < -6)
        {
            Destroy(gameObject);
        }
    }

   public void SetDirectionAndSpeed(Vector2 direction, float speed)
    {
        rbMouse.velocity = direction * speed;
       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            int forceSquare = 1;
            if (other.name.Contains("Square2"))
            {
                forceSquare = 1024;
            }
            else if (other.name.Contains("Square3"))
            {
                forceSquare = 1048576;
            }
            else if (other.name.Contains("Square4")) // Pas de square 4 5 6
            {
                forceSquare = (int)Math.Pow(1024, 3);
            }
            else if (other.name.Contains("Square5"))
            {
                forceSquare = (int)Math.Pow(1024, 4);
            }
            else if (other.name.Contains("Square6"))
            {
                forceSquare = (int)Math.Pow(1024, 5);
            }
            else if (other.name.Contains("Pop-up"))
            {
                forceSquare = -10;
            }
            // vérifier quelle type de cible, selon la cible appliquer un multiplicateur à la force
            scriptClickerManager.Square(scriptClickerManager.forceMouse*forceSquare);
            Destroy(gameObject);
        }
        else if (other.name.Contains("Upgrade")) // force
        {
            scriptClickerManager.Upgrade();
            Destroy(gameObject);
        }
        else if (other.name.Contains("Autoclick")) // autoclick
        {
            scriptClickerManager.Autoclick();
            Destroy(gameObject);
        }
        else if (other.name.Contains("Precision")) // precision
        {
            scriptClickerManager.Precision();
            Destroy(gameObject);
        }
        else if (other.name.Contains("Range")) // range
        {
            scriptClickerManager.Range();
            Destroy(gameObject);
        }
    }
}
