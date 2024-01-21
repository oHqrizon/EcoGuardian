using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeScript : MonoBehaviour
{
    private float speed = 3f;
    private float health = 3f;
    private bool rotated = false;
    private bool active = false;
    private bool isAttacking;
    private float playerX;
    private float enemyX;

    private Animator animator;
    private Transform player;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        animator.Play("SlimeIdle");
    }

    void Update()
    {   
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= 5f && !active) {
            active = true;
        }

        if (active && !isAttacking) {
            moveSlime();
        }

        RotateTowardsPlayer();
    }

    void RotateTowardsPlayer()
    {
        playerX = player.position.x;
        enemyX = transform.position.x;

        if (enemyX < playerX && !rotated)
        {
            turnBack();
            rotated = true;
        }
        else if (enemyX > playerX && rotated)
        {
            turn();
            rotated = false;
        }
    }

   void turn() {
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    void turnBack() {
        transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    public void takeHit()
    {
        health-=1;

        if (health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        // Play the death animation
        animator.Play("SlimeDie");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }

    IEnumerator slimeAttack() {
        isAttacking = true;
        animator.Play("SlimeAttack");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        isAttacking = false;
    }

    void moveSlime() {
        animator.Play("SlimeRun");

        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);

        // Delta time for smoother movement
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") && !isAttacking) {
            StartCoroutine(slimeAttack());
        }
    }

}