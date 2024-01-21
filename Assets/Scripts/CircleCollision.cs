using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleCollision : MonoBehaviour
{
    private PlayerAbilities playerState;

    void Start() {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        playerState = playerObject.GetComponent<PlayerAbilities>();
    }
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") && !playerState.isHurting) {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(1f);
            Destroy(gameObject);
        }

        else if (collision.CompareTag("Player") && gameObject != null) {
            Destroy(gameObject);
        }
        
    }
    
}
