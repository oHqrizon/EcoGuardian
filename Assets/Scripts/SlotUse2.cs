using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotUse2 : MonoBehaviour
{
    
    private Inventory inventory;
    private PlayerHealth playerHealth;
    private PlayerAbilities playerAbilities;
    public bool placed = false;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        playerAbilities = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAbilities>();

    }

    public void useItem() {
        foreach (Transform child in transform) {
            GameObject childObject = child.gameObject;
            
            Image image = childObject.GetComponent<Image>();
            Sprite sprite = image.sprite;
            string spriteName = sprite.name;

            if (spriteName.Equals("heart") && playerHealth.getHealth() <= 5) {
                playerHealth.heal(1f);
                GameObject.Destroy(child.gameObject);
                inventory.isFull[1] = false;
            } 
            else if (spriteName.Equals("heart"))
            {
                GameObject.Destroy(child.gameObject);
                inventory.isFull[1] = false;
            }
            else if (spriteName.Equals("flower") && playerAbilities.canPlace == true)
            {
                GameObject.Destroy(child.gameObject);
                inventory.isFull[1] = false;
                placed = true;
            } 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey((KeyCode.Alpha2))) {
            useItem();
        }

    }
}
