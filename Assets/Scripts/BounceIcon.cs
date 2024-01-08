using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceIcon : MonoBehaviour
{
    private float maxScale = 1.2f;
    //private float minScale = 0.8f;
    private float clickDuration = 0.9f;
    public bool bounceActive = false;
    private float countdownTimer;
    private Vector3 originalScale;

    void Start()
    {
        countdownTimer = clickDuration;
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (bounceActive == true)
        {
            countdownTimer -= Time.deltaTime;
            Vector2 originalScale = transform.localScale;
            float clickTime = Mathf.PingPong(Time.time / clickDuration, 1);
            float scaleMultiplier = 1 + Mathf.Sin(clickTime * Mathf.PI) * (maxScale - 1);
            transform.localScale = originalScale * scaleMultiplier;
        }

        if (countdownTimer <= 0)
        {
            bounceActive = false;
            transform.localScale = originalScale;
            countdownTimer = clickDuration;
        }

    }
}

