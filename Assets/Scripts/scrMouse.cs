using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrMouse : MonoBehaviour
{
    public Rigidbody2D rbMouse;
    // Start is called before the first frame update
    void Start()
    {
        
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
}
