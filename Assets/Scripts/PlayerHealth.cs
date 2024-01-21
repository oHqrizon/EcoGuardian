using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public float startingHealth;
    
    public float currentHealth { get; private set; }

    private void Awake() {

        currentHealth = startingHealth;
    }

    // Update is called once per frame
    public void TakeDamage(float dmg) {

        currentHealth = Mathf.Clamp(currentHealth - dmg, 0, startingHealth+5);
        
    }

    public void heal(float amount) {
        setHealth(currentHealth + amount);
    }

    public float getHealth() {
        
        return currentHealth;
    }

    public void setHealth(float newHealth) {
        currentHealth = newHealth;
    }

}
