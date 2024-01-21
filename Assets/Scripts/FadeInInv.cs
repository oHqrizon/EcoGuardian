using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    private float fadeDuration;
    private Image image;
    private Color targetColor;
    private Color initialColor;
    private float currentTime;

    // Start is called before the first frame update
    void Start()
    {
        fadeDuration = 2f;
        currentTime = 0f;

        image = GetComponent<Image>();

        initialColor = image.color;
        initialColor.a = 0f;

        image.color = initialColor;

        targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 1f);

        StartCoroutine(delay());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator slowFade() {
        while (currentTime < fadeDuration) {
            currentTime += Time.deltaTime;

            float interFactor = currentTime / fadeDuration;
            Color lerpColor = Color.Lerp(initialColor, targetColor, interFactor);

            image.color = lerpColor;

            yield return null;
        }

        enabled = false;
    }

    IEnumerator delay() {
        yield return new WaitForSeconds(5f);

        StartCoroutine(slowFade());
    }
}
