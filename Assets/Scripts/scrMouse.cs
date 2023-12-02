using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrMouse : MonoBehaviour
{
    public Rigidbody2D rbMouse;
    public scrClickerManager scriptClickerManager;


    // Start is called before the first frame update
    void Start()
    {
        scriptClickerManager = FindObjectOfType<scrClickerManager>();
    }

    // Update is called once per frame
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
        if (other.name.Contains("Square"))
        {
            //fonction clic avec force du clic prise en compte
            scriptClickerManager.Square(scriptClickerManager.forceMouse);
            Destroy(gameObject);
        }
        else if (other.name.Contains("Upgrade"))
        {
            
            scriptClickerManager.Upgrade();
            Destroy(gameObject);
        }
    }

}
