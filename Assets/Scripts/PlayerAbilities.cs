using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerAbilities : MonoBehaviour
{
    private float speed;
    private float jumpForce;
    private float dashForce;
    private float dashCooldown;
    private float dashDuration;
    private int jumpsLeft;
    private bool isJumping;
    private bool isDashing;
    public bool isHurting;
    private bool isDashCooldown;
    private bool rotated;
    public bool canInput;
    private float delay;
    public bool initalTouch = false;
    private float moveHorizontal;
    private bool isHoldingKey;
    private bool firstAttack;
    private bool isAttacking;
    private bool isHit;
    public bool canPlace;
    private Vector2 lastDirection;
    private Rigidbody2D rb;
    private BatHealth batHealth, batHealth1, batHealth2, batHealth3, batHealth4;
    private SpiderHP spiderHP;
    private GameObject batGameObject, batGameObject1, batGameObject2, batGameObject3, batGameObject4;
    private SpriteRenderer spriteRenderer;
    private Transform player;
    private Animator animator;
    public ParticleSystem dust;
    private PlayerHealth playerHealth;
    public AudioSource thud;
    public AudioSource slash;
    public AudioSource hit;
    private CircleCollider2D circleCollider;
    private PolygonCollider2D polygonCollider;
    private ChestOpen chestOpen;
    public bool initialLand;
    private Animator spiderAni;
    private bool isSpiderHit;
    private SlimeScript slime;
    private SlimeScript2 slime1;
    private SlimeScript3 slime2;

    void Start()
    {
        thud.Stop();

        lastDirection = Vector2.right;

        rotated = false;
        isJumping = false;
        isDashing = false;
        isDashCooldown = false;
        canInput = false;
        rotated = false;
        isHoldingKey = false;
        firstAttack = false;
        isAttacking = false;
        isHurting = false;
        isHit = false;
        canPlace = false;
        initialLand = false;
        dashDuration = 0.3f;
        dashForce = 8f;
        dashCooldown = 1f;
        speed = 8f;
        jumpForce = 8f;
        jumpsLeft = 1;
        delay = 5.5f;

        batGameObject = GameObject.Find("Triangle");
        batGameObject1 = GameObject.Find("Triangle1");
        batGameObject2 = GameObject.Find("Triangle2");
        batGameObject3 = GameObject.Find("Triangle3");
        batGameObject4 = GameObject.Find("Triangle4");

        player = GameObject.FindGameObjectWithTag("Player").transform;
        chestOpen = GameObject.FindGameObjectWithTag("Chest1").GetComponent<ChestOpen>();
        spiderHP = GameObject.FindGameObjectWithTag("Spider").GetComponent<SpiderHP>();
        spiderAni = GameObject.FindGameObjectWithTag("Spider").GetComponent<Animator>();
        slime = GameObject.FindGameObjectWithTag("Slime").GetComponent<SlimeScript>();
        slime1 = GameObject.FindGameObjectWithTag("Slime1").GetComponent<SlimeScript2>();
        slime2 = GameObject.FindGameObjectWithTag("Slime2").GetComponent<SlimeScript3>();

        rb = GetComponent<Rigidbody2D>();

        batHealth = batGameObject.GetComponent<BatHealth>();
        batHealth1 = batGameObject1.GetComponent<BatHealth>();
        batHealth2 = batGameObject2.GetComponent<BatHealth>();
        batHealth3 = batGameObject3.GetComponent<BatHealth>();
        batHealth4 = batGameObject4.GetComponent<BatHealth>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHealth>();
        circleCollider = GetComponent<CircleCollider2D>();
        polygonCollider = GetComponent<PolygonCollider2D>();

        circleCollider.enabled = false;
        polygonCollider.enabled = false;

        StartCoroutine(enableInputAfterDelay());

    }
    void Update()
    {
        if (initalTouch == false)
        {
            animator.SetTrigger("PlayerFall");
        }
        if (initalTouch == true && initialLand == false)
        {
            StartCoroutine(land());
        }

        if (gameObject.transform.position.y >= 50)
        {
            rb.gravityScale = 1f;
        }
        else
        {
            rb.gravityScale = 3.5f;
        }

        if (canInput)
        {
            if (Input.GetMouseButtonDown(0) && !isAttacking && !isHurting && !isDashing)
            {
                StartCoroutine(playAttackAni());
                slash.Play();
            }
            else if (moveHorizontal != 0 && !isAttacking && !isHurting && !isDashing)
            {
                animator.Play("PlayerRun");
            }
            else if (moveHorizontal == 0 && !isAttacking && !isHurting && !isDashing)
            {
                animator.Play("PlayerIdle");
            }

            // Handle non-physics-related input in Update
            if (Input.GetKeyDown(KeyCode.E) && !isDashing && !isDashCooldown && !isAttacking && !isHurting)
            {
                if (chestOpen.hasOpened == true)
                {
                    StartCoroutine(Dash(lastDirection));
                }
            }

            if (Input.GetKey(KeyCode.D) && !isHoldingKey)
            {
                isHoldingKey = true;
                lastDirection = Vector2.right;

                // Rotates depending on which side the enemy is on
                if (rotated)
                {
                    turn();
                    rotated = false;
                }

            }
            else if (Input.GetKey(KeyCode.A) && !isHoldingKey)
            {
                isHoldingKey = true;
                lastDirection = Vector2.left;

                // Rotates depending on which side the enemy is on
                if (!rotated)
                {
                    turnBack();
                    rotated = true;
                }

            }
            else
            {
                isHoldingKey = false;
            }

            if (!isJumping && jumpsLeft > 0 && (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W)))
            {
                jumpsLeft--;
                dust.Play();
                isJumping = true;
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Sqrt(2 * jumpForce * Mathf.Abs(Physics2D.gravity.y)));

                if (jumpsLeft == 1)
                {
                    StartCoroutine(DelayedJump());
                }

            }
        }
    }

    IEnumerator playAttackAni()
    {

        if (firstAttack == false && !isHurting && !isAttacking)
        {
            isAttacking = true;
            animator.Play("PlayerAttack1");

            yield return new WaitForSeconds(0.1f);

            polygonCollider.enabled = true;
            firstAttack = true;
        }
        else if (firstAttack == true && !isHurting && !isAttacking)
        {
            isAttacking = true;
            animator.Play("PlayerAttack2");

            yield return new WaitForSeconds(0.1f);

            circleCollider.enabled = true;
            firstAttack = false;
        }

        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

        float waitTime = currentState.length * 0.4f;

        yield return new WaitForSeconds(waitTime);

        circleCollider.enabled = false;
        polygonCollider.enabled = false;

        yield return new WaitForSeconds(currentState.length - waitTime);

        isAttacking = false;
        isHit = false;

    }

    public IEnumerator playHurtAni()
    {
        isHurting = true;
        speed = 8f;
        animator.Play("PlayerHit");
        hit.Play();

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        isHurting = false;
        isSpiderHit = false;
    }

    IEnumerator land()
    {
        animator.Play("PlayerInitalLand");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        initialLand = true;
    }

    void turn()
    {
        Vector3 rotater = new Vector3(transform.rotation.x, 0f, transform.rotation.x);
        transform.rotation = Quaternion.Euler(rotater);
    }

    void turnBack()
    {
        Vector3 rotater = new Vector3(transform.rotation.x, 180, transform.rotation.x);
        transform.rotation = Quaternion.Euler(rotater);
    }

    void FixedUpdate()
    {

        if (canInput)
        {
            if (!isHoldingKey)
            {
                moveHorizontal = Input.GetAxis("Horizontal");
                rb.velocity = new Vector2(moveHorizontal * speed, rb.velocity.y);
            }

            if (isDashing)
            {

                rb.AddForce(lastDirection * dashForce, ForceMode2D.Impulse);
            }
        }
    }

    IEnumerator DelayedJump()
    {
        yield return new WaitForSeconds(0.4f);
        isJumping = false;
    }


    public IEnumerator enableInputAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        canInput = true;
    }

    IEnumerator Dash(Vector2 dashDirection)
    {
        isDashing = true;
        isDashCooldown = true;
        float startTime = Time.time;
        animator.Play("PlayerDash");
        while (Time.time - startTime < dashDuration)
        {
            yield return null;
        }

        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        isDashCooldown = false;

    }

    IEnumerator spiderHit() {
        yield return new WaitForSeconds(0.25f);
        StartCoroutine(playHurtAni());
        playerHealth.TakeDamage(2f);
    }

    void CreateDust()
    {
        dust.Play();
    }

    void OnTriggerEnter2D(Collider2D collision) {
        Collider2D collider = collision.GetComponent<Collider2D>();
        if (collision.gameObject.tag == "Platform")
        {
            isJumping = false;
            initalTouch = true;
            speed = 8f;
            thud.Play();
            if (batHealth.dead)
            {
                jumpsLeft = 2;
            }
            else
            {
                jumpsLeft = 1;
            }

        }
        if (collision.CompareTag("Fireball")) {
            speed = 3f;
            if (!isAttacking && !isHurting)
            {
                StartCoroutine(playHurtAni());
            }

        }

        if (collision.CompareTag("Slime")) {
            if (!isAttacking && !isHurting) {
                StartCoroutine(playHurtAni());
                playerHealth.TakeDamage(1f);
            }
        }

        if (collision.CompareTag("Slime1")) {
            if (!isAttacking && !isHurting) {
                StartCoroutine(playHurtAni());
                playerHealth.TakeDamage(1f);
            }
        }

        if (collision.CompareTag("Slime2")) {
            if (!isAttacking && !isHurting) {
                StartCoroutine(playHurtAni());
                playerHealth.TakeDamage(1f);
            }
        }
			
        if (collision.CompareTag("Bat") && !isHurting && isAttacking && !isHit) {
            isHit = true;
            batHealth.takeHit();
        }

        if (collision.CompareTag("Bat1") && !isHurting && isAttacking && !isHit) {
            isHit = true;
            batHealth1.takeHit();
        }

        if (collision.CompareTag("Bat2") && !isHurting && isAttacking && !isHit) {
            isHit = true;
            batHealth2.takeHit();
        }
        if (collision.CompareTag("Bat3") && !isHurting && isAttacking && !isHit)
        {
            isHit = true;
            batHealth3.takeHit();
        }
        if (collision.CompareTag("Bat4") && !isHurting && isAttacking && !isHit)
        {
            isHit = true;
            batHealth4.takeHit();
        }

        if (collision.CompareTag("Slime") && !isHurting && isAttacking && !isHit)
        {
            isHit = true;
            slime.takeHit();
        }

        if (collision.CompareTag("Slime1") && !isHurting && isAttacking && !isHit)
        {
            isHit = true;
            slime1.takeHit();
        }

        if (collision.CompareTag("Slime2") && !isHurting && isAttacking && !isHit)
        {
            isHit = true;
            slime2.takeHit();
        }

        if (collision.gameObject.tag == "Lava")
        {
            speed = 3f;
        }
		
        if (collision.CompareTag("Spider") & !isHurting && isAttacking && !isHit) {
                isHit = true;
                spiderHP.takeHit();
        }

        if (collision.CompareTag("Spider") ) {
            AnimatorStateInfo stateInfo = spiderAni.GetCurrentAnimatorStateInfo(0);
            string currentAnimation = stateInfo.IsName("Base Layer.Idle") ? "Idle" : stateInfo.fullPathHash.ToString();
            if (currentAnimation.Equals("1458213094") && !isHurting && !isSpiderHit) {
                StartCoroutine(spiderHit());
                isSpiderHit = true;
            }

        }

        if (collision.CompareTag("MiniSpider")) {
            if (!isAttacking && !isHurting) {
                StartCoroutine(playHurtAni());
                playerHealth.TakeDamage(0.5f);
            }
        }

    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Seed"))
        {
            canPlace = true;
        }
        else
        {
            canPlace = false;
        }
    }

}

