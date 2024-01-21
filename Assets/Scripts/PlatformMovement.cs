using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public float speed = 2.0f;  // Adjust the speed as needed
    public float distance = 3.0f;  // Adjust the distance the platform should move

    private Vector2 startPosition;
    private float initialPosition;
    private float timeElapsed;

    void Start()
    {
        startPosition = transform.position;
        initialPosition = startPosition.x;
    }

    void Update()
    {
        MovePlatform();
    }

    void MovePlatform()
    {
        timeElapsed += Time.deltaTime;

        // Calculate the new position of the platform using a sine function for smooth motion
        float newPosition = initialPosition + Mathf.Sin(timeElapsed * speed) * distance;
        Vector2 newPositionVector = new Vector2(newPosition, transform.position.y);

        // Move the platform
        transform.position = newPositionVector;
    }
}