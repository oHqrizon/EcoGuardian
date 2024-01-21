using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.Mathematics;
using UnityEngine;

public class DashText : MonoBehaviour
{
    public Animator animatorText1;
    public Animator animatorText2;
    public Transform player;
    private int counter;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= 8.5f && counter!=1)
         {
            animatorText1.Play("textUnlock");
            animatorText2.Play("textUnlock");
            counter =1;
        }
    }
}
