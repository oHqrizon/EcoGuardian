using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{

    private Inventory inventory;
    public GameObject item;
    private bool canPickup;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

        canPickup = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator stopPickUp() {
        canPickup = false;
        yield return new WaitForSeconds(0.5f);
        canPickup = true;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") && canPickup) {
            for (int i = 0; i < inventory.slots.Length; i ++) {
                if (inventory.isFull[i] == false) {
                    Instantiate(item, inventory.slots[i].transform, false);
                    Destroy(gameObject);
                    inventory.isFull[i] = true;
                    StartCoroutine(stopPickUp());
                    break;
                }
            }
        }
        
    }

}
