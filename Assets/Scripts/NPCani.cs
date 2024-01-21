using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCani : MonoBehaviour
{
    private BatHealth batHealth;
    private GameObject batGameObject;
    private GameObject ExplosionGameObject;
    private bool hasWalked;
    private SpriteRenderer spriteRenderer;
    private Vector2 initialPosition;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        batGameObject = GameObject.Find("Triangle");
        batHealth = batGameObject.GetComponent<BatHealth>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        
        if (batHealth.getHealth() == 0 && hasWalked == false)
        {
            animator.SetTrigger("Start");
            hasWalked = true;

        }
        else if (hasWalked)
        {
            animator.SetTrigger("Walk");
        }
        else
        {
            animator.SetTrigger("Idle");
            
        }

        if (Vector2.Distance(transform.position, initialPosition) > 0.1f) {
            spriteRenderer.flipX = true;
        }

    }

}
