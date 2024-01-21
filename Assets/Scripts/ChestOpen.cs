using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOpen : MonoBehaviour
{
    private Animator animator;
    public bool hasOpened;
    public Animator animatorText;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("Idle");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (hasOpened == false)
            {
                StartCoroutine(playChestOpen());
                animatorText.Play("TextChest");
            }
            else
            {
                animator.Play("FinshedOpen", 0);
            }

        }
    }

    IEnumerator playChestOpen()
    {
        animator.Play("ChestOpen1");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        hasOpened = true;
    }
}


