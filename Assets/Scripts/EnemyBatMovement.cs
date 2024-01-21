using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Animator animator;

    // Adjustable speed var and booleans to regulate rotations
    private float speed = 6f;
    private Transform player;
    private bool rotated = false;
    private SpriteRenderer spriteRenderer;
    private bool batFall = false;
    private Vector2 originalPos;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("BatIdle1");

        // Finds player object
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();

        originalPos = transform.position;
    }

    void FixedUpdate() {

        if (batFall) {
            moveBat();
        }
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        if (distanceToPlayer <= 8f) {
            if (!batFall)
            {
                StartCoroutine(batFalling());
            }   
            else
            {  
                animator.Play("BatFly");
            }
            
        } else if (distanceToPlayer >= 10f) {
            reset();
        }
        
        float playerX = player.position.x;
        float enemyX = transform.position.x;

        // Rotates depending on which side the enemy is on
        if (enemyX < playerX && !rotated) {
            turnBack();
            rotated = true;
        }
        else if (enemyX > playerX && rotated) {
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

    void reset() {
        Vector2 direction = (originalPos - (Vector2)transform.position).normalized;
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    void moveBat() {
        Vector3 offset = new Vector3(3f, 1f, 0f);

        if (rotated) 
        {
            offset.x = -offset.x;
        }

        Vector3 targetPosition = player.position + offset;

        // Delta time for smoother movement
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    IEnumerator batFalling()
    {
        animator.Play("BatFall");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        batFall = true;
    }

    }
