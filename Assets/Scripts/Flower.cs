using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    [SerializeField] ChestOpen check;
    private SpriteRenderer sr;
    private Color initialColor;
    private bool isFloating;
    private Vector2 initialPosition;
    private PolygonCollider2D polygonCollider;
    private float targetY;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        polygonCollider = GetComponent<PolygonCollider2D>();

        initialColor = sr.color;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0f);

        initialPosition = transform.position;

        polygonCollider.enabled = false;
        isFloating = false;

    }

    // Update is called once per frame
    void Update()
    {

        if (check.hasOpened == true && !isFloating)
        {
           StartCoroutine(fadeInAndFloatUp());
        }

        if (isFloating) {
            polygonCollider.enabled = true;
        }   
    }

    IEnumerator fadeInAndFloatUp() {

        float elapsedTime = 0f;
        float duration = 2f;
        float startY = initialPosition.y;
        targetY = initialPosition.y + 2.5f;

        while (elapsedTime < duration) {

            float a = Mathf.Lerp(0f, 1f, elapsedTime/duration);
            sr.color = new Color(initialColor.r, initialColor.g, initialColor.b, a);

            float newY = Mathf.Lerp(startY, targetY, elapsedTime/duration);
            transform.position = new Vector2(transform.position.x, newY);

            elapsedTime += Time.deltaTime;
            
            yield return null;
        }

        sr.color = initialColor;
        transform.position = new Vector2(transform.position.x, targetY);

        isFloating = true;  
        
    }

}
