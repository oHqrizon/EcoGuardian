using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{

    private Vector2 startPos;
    private bool checkValid;
    private PlayerHealth playerHealth;
    private Healthbar healthbar;
    private PlayerAbilities playerAbilities;
    private Rigidbody2D rigidBody;
    public AudioSource wind;
    private ChestOpen2 chestOpen2;
    [SerializeField] PlayerAbilities touch;

    // Start is called before the first frame update
    void Start()
    {
        checkValid = false;
        startPos = transform.position;

        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        healthbar = GameObject.FindObjectOfType<Healthbar>();
        playerAbilities = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAbilities>();
        rigidBody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        chestOpen2 = GameObject.FindGameObjectWithTag("Chest2").GetComponent<ChestOpen2>();
    }

    // Update is called once per frame
    void Update()
    {

        if (playerHealth.getHealth() == 6 && !checkValid)
        {
            checkValid = true;
        }

        if (playerHealth.getHealth() == 0 && checkValid && !chestOpen2.hasOpened)
        {
            rigidBody.velocity = Vector2.zero;
            playerAbilities.canInput = false;

            StartCoroutine(respawn());
            StartCoroutine(playerAbilities.enableInputAfterDelay());
            
        }

        if (playerHealth.getHealth() == 0 && checkValid && chestOpen2.hasOpened) {
            
            rigidBody.velocity = Vector2.zero;
            StartCoroutine(respawn2());
        }
    }

    IEnumerator respawn()
    {
        transform.position = startPos;

        touch.initalTouch = false;
        touch.initialLand = false;
        wind.Play();
        checkValid = false;

        StartCoroutine(healthbar.fillOverTime());
        
        yield break;
        
    }

    IEnumerator respawn2() {
        
        Vector2 respawnPoint = new Vector2(242f, 18f);
        transform.position = respawnPoint;

        checkValid = false;
        StartCoroutine(healthbar.fillOverTime());

        yield break;
    }

}

