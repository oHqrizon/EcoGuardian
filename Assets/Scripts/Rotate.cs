using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    private Transform player;
    private float playerX;
    private float enemyX;
    private bool rotated = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.transform;
    }

    void turn() {
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    void turnBack() {
        transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
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
    }
}
