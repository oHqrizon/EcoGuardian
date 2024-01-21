using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateFall : MonoBehaviour
{
    private BatHealth batHealth;
    private GameObject batGameObject;

    private float fallDelay = 0.5f;
    [SerializeField] private Rigidbody2D rb;

    void Start()
    {
        batGameObject = GameObject.Find("Triangle");
        batHealth = batGameObject.GetComponent<BatHealth>();
    }

    // Update is called once per frame
    void Update()
    {
     if (batHealth.getHealth() == 0)
        {
            StartCoroutine(Fall());
        }   
    }
    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallDelay);
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 2f;

    }
}
