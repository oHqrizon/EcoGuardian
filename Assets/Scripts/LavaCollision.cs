using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaCollision : MonoBehaviour
{
    
    private bool isDamaging = false;
    private Animator playerAnimator;
    private PlayerAbilities playerMovement;
    public AudioSource hit;

    void Start() {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        playerAnimator = playerObject.GetComponent<Animator>();
        
        playerMovement = playerObject.GetComponent<PlayerAbilities>();
    }

    void OnTriggerStay2D(Collider2D collision) {
        
        if (collision.CompareTag("Player") && !isDamaging) {
            StartCoroutine(delayBeforeDmg(collision));
        }
    }

    IEnumerator delayBeforeDmg(Collider2D playerCollider) {
        
        isDamaging = true;
        yield return new WaitForSeconds(0.75f);

        PlayerHealth playerHealth = playerCollider.GetComponent<PlayerHealth>();
        playerHealth.TakeDamage(2f);

        StartCoroutine(playHurtAni());
       
        isDamaging = false;
    }

    IEnumerator playHurtAni() {
        playerMovement.isHurting = true;

        playerAnimator.Play("PlayerHit");
        hit.Play();
        yield return new WaitForSeconds(playerAnimator.GetCurrentAnimatorStateInfo(0).length);
        
        playerMovement.isHurting = false;
        
    }
}
