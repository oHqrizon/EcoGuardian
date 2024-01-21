using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAttack : MonoBehaviour
{
    private Transform player;
    private Animator animator;
    private PlayerHealth playerHealth;
    private SpiderHP spiderHP;
    private SpiderSpawnManager spiderSpawn;
    private bool triggered = false;
    private float speed = 6f;
    private bool rotated = false;
    public bool inRange = false;
    private float xOffset = 4f;
    private float playerX;
    private float enemyX;
    private float cd1 = 0f;
    private bool attacked = false;
    private Vector3 startPos;
    private bool phase2 = false;
    public bool jumped = false;
    private bool phase2Done = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        spiderSpawn = GetComponent<SpiderSpawnManager>();
        spiderHP = GetComponent<SpiderHP>();
        animator = GetComponent<Animator>();
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > 20f && !inRange) {
            animator.Play("SpiderIdle");
        } else if (distanceToPlayer <= 20f && !triggered) {
            StartCoroutine(delayAfterTrigger());
        }

        if (playerHealth.getHealth() == 0f && spiderHP.dead == false) {
            transform.position = Vector3.MoveTowards(transform.position, startPos, speed * Time.deltaTime);
        }

        playerX = player.position.x;
        enemyX = transform.position.x;

        // Rotates depending on which side the enemy is on
        if (enemyX < playerX && !rotated)
        {
            turn();
            rotated = true;
        }
        else if (enemyX > playerX && rotated)
        {
            turnBack();
            rotated = false;
        }

        if (triggered && inRange && !phase2)
        {
            moveSpider();
        }

        if (triggered && distanceToPlayer <= 7f && cd1 <= 0f && spiderHP.dead == false && !phase2) {
           StartCoroutine(attackPlayer());
           attacked = true;
           cd1 = 2f;
        }

        if (cd1 > 0f && attacked) {
            cd1 -= Time.deltaTime;
        }

        if (spiderHP.getHealth() == 10f && !phase2Done) {
            phase2 = true;
        }

        if (phase2) {
            phase();
        }

        if(playerHealth.getHealth() == 0f) {
            phase2Done = false;
            phase2 = false;
            inRange = false;
            triggered = false;
            rotated = false;
            jumped = false;
            cd1 = 0f;
            StartCoroutine(reset(2f));
        }
    }

    IEnumerator reset(float duration)
{
    float timer = 0f;
    Vector2 originalPosition = new Vector2(transform.position.x, startPos.y);

    while (timer < duration)
    {
        transform.position = Vector2.Lerp(transform.position, originalPosition, timer / duration);
        timer += Time.deltaTime;
        yield return null;
    }

    transform.position = originalPosition;
}

    void phase() {
        if (!jumped) {
            StartCoroutine(jump(1f));
            jumped = true;        
        } else {
            StartCoroutine(attackPlayer2());
        }

        if (spiderSpawn.count >= 3) {
            StartCoroutine(moveSpiderDown(2f));
            phase2 = false;
            phase2Done = true;
            animator.Play("SpiderRun");
            
        }
    }

    IEnumerator moveSpiderDown(float duration) {
        float timer = 0f;
        Vector2 originalPosition = new Vector2(transform.position.x, startPos.y);

        while (timer < duration) {
            transform.position = Vector2.Lerp(transform.position, originalPosition, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

    transform.position = originalPosition;
    }

    IEnumerator jump(float duration) {

         float timer = 0f;
    
        while (timer < duration)
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * 3f, ForceMode2D.Impulse);
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0f);
    }

    IEnumerator attackPlayer2() {
        animator.Play("SpiderAttack2");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
    }

    void turn() {
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    void turnBack() {
        transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    IEnumerator delayAfterTrigger() {
        animator.Play("SpiderJump");
        yield return new WaitForSeconds(1.133333f);
        animator.Play("SpiderRun");
        triggered = true;
        inRange = true;
  
    }
    
    void moveSpider() {
        float newxOffset = rotated ? -xOffset: xOffset;
        float targetX = player.position.x + newxOffset;

        Vector3 targetPosition = new Vector3(targetX, transform.position.y, player.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        
    }

    IEnumerator attackPlayer() {
        animator.Play("SpiderAttack1");
        yield return new WaitForSeconds(1f);
        animator.Play("SpiderRun");
    }
}
