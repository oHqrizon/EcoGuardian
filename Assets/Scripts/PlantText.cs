using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantText : MonoBehaviour
{
    public Animator animator;
    public Transform player;
    private SlotUse1 slot1;
    private SlotUse2 slot2;
    private SlotUse3 slot3;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        slot1 = GameObject.FindGameObjectWithTag("Slot1").GetComponent<SlotUse1>();
        slot2 = GameObject.FindGameObjectWithTag("Slot2").GetComponent<SlotUse2>();
        slot3 = GameObject.FindGameObjectWithTag("Slot3").GetComponent<SlotUse3>();
    }

    // Update is called once per frame
    void Update()
    {
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (slot1.placed == true || slot2.placed == true || slot3.placed == true)
        {
            animator.Play("PlantFadeout");
        }
        else
        {
            if (distanceToPlayer <= 8.5f)
            {
                animator.Play("PlantTextfade");
            }
            else
            {
                animator.Play("PlantFadeout");
            }
        }
        
    } 
}
