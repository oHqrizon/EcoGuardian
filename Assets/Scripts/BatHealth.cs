using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatHealth : MonoBehaviour
{
    private int maxHits = 3;
    private int currentHits;
    public bool dead;

    // Start is called before the first frame update
    void Start()
    {
        currentHits = 0;
        dead = false;

    }

    public void takeHit() {

        if (dead) return;

        currentHits ++;

        if (currentHits >= maxHits) {
            
            die();

        }
    }

    public void die() {

        if (dead) return;

        dead = true;

        gameObject.SetActive(false);
        
    }

    public int getHealth() {
        return maxHits - currentHits;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
