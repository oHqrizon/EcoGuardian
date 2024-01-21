using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    public GameObject dialogueBox;
    private PlayerAbilities playerMovementScript;
    private int index;
    private Rigidbody2D playerRigidbody;
    private bool isStarted = false;
    private BatHealth batHealth;
    private GameObject batGameObject;
    private GameObject playerObject;
    private GameObject ExplosionGameObject;
    public Animator animatorText;

    // Start is called before the first frame update
    void Start()
    {
        batGameObject = GameObject.Find("Triangle");
        ExplosionGameObject = GameObject.Find("Explosion");
        playerObject = GameObject.FindGameObjectWithTag("Player");

        ExplosionGameObject.SetActive(false);
        dialogueBox.SetActive(false);

        batHealth = batGameObject.GetComponent<BatHealth>();
        
        playerMovementScript = playerObject.GetComponent<PlayerAbilities>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isStarted && batHealth.getHealth() == 0) {
            if (textComponent.text == lines[index]) {
                nextLine();
            } else {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && batHealth.getHealth() == 0) {
            startDialogue();
            dialogueBox.SetActive(true);
        }
    }

    void startDialogue() {
        isStarted = true;
        index = 0;
        textComponent.text = string.Empty;
        StartCoroutine(typeLine());

        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;

        playerRigidbody = playerObject.GetComponent<Rigidbody2D>();
        playerRigidbody.velocity = Vector2.zero;
        playerRigidbody.isKinematic = true;

        playerMovementScript.enabled = false;
    }

    IEnumerator typeLine() {
        foreach (char c in lines[index].ToCharArray()) {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void nextLine() {
        if (index < lines.Length - 1) {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(typeLine());
        } else {   
            dialogueBox.SetActive(false);
            gameObject.SetActive(false);
            ExplosionGameObject.SetActive(true);
            animatorText.Play("textUnlock");

            playerMovementScript.enabled = true;
            playerRigidbody.isKinematic = false;
            isStarted = false;
        }
    }
}