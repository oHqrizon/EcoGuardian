using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInFlower : MonoBehaviour
{
    private SpriteRenderer sr;
    private Color initialColor;
    private GameObject slotUseObject;
    private GameObject slotUseObject1;
    private GameObject slotUseObject2;
    private SlotUse1 slot1;
    private SlotUse2 slot2;
    private SlotUse3 slot3;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        slotUseObject = GameObject.Find("Slot1");
        slot1 = slotUseObject.GetComponent<SlotUse1>();

        slotUseObject1 = GameObject.Find("Slot2");
        slot2 = slotUseObject1.GetComponent<SlotUse2>();

        slotUseObject2 = GameObject.Find("Slot3");
        slot3 = slotUseObject2.GetComponent<SlotUse3>();
        
        initialColor = sr.color;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0f);
    }

    IEnumerator fadeInAndFloatUp() {

        float elapsedTime = 0f;
        float duration = 2f;

        while (elapsedTime < duration) {

            float a = Mathf.Lerp(0f, 1f, elapsedTime/duration);
            sr.color = new Color(initialColor.r, initialColor.g, initialColor.b, a);

            elapsedTime += Time.deltaTime;
            
            yield return null;
        }

        sr.color = initialColor;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (slot1.placed == true || slot2.placed == true || slot3.placed == true) {
            StartCoroutine(fadeInAndFloatUp());
        } 
    }
}
