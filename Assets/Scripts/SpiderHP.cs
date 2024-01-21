using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpiderHP : MonoBehaviour
{
    private int maxHits = 20;
    private int currentHits = 0;
    public bool dead = false;
    private bool filled = false;

    private Transform player;
    public Slider healthBar;
    private PlayerHealth playerHealth;
    private Animator animator;
    private float fillSpeed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        animator = GetComponent<Animator>();

        healthBar.value = 0f;
        healthBar.gameObject.SetActive(false);
        
    }

    public void takeHit() {

        if (dead) return;

        currentHits ++;

        if (currentHits >= maxHits) {
            die();
        }

        StartCoroutine(slowHealthBar());

    }

    public void die() {
        dead = true;
        StartCoroutine(killSpider());
    }

    IEnumerator killSpider() {
        animator.Play("SpiderDeath");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        removeHealthBar();
        Destroy(gameObject);
        
        
    }

    IEnumerator slowHealthBar() {
        
        float targetValue = (float) (maxHits - currentHits) / maxHits;

        while (healthBar.value > targetValue) {
            healthBar.value -= fillSpeed * Time.deltaTime;
            yield return null;
        }

        healthBar.value = targetValue;
    }

    IEnumerator fillHealthBar(float targetValue) {
        
        float startValue = healthBar.value;

        while (healthBar.value < targetValue) {
            healthBar.value += fillSpeed * Time.deltaTime;
            yield return null;
        }

        healthBar.value = targetValue;
    }

    void removeHealthBar()
    {
        healthBar.gameObject.SetActive(false);
        filled = false;
    }

    public float getHealth() {
        return maxHits - currentHits;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= 75f && !filled && !dead) {
            filled = true;
            healthBar.gameObject.SetActive(true);
            StartCoroutine(fillHealthBar(1f));
        }

        if (playerHealth.getHealth() == 0f && !dead) {
            currentHits = 0;
            healthBar.value = 0f;
            removeHealthBar();
        }
    }
}
