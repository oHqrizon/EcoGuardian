using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float FollowSpeed = 8f;
    public float yOffset;
    public Transform target;

    [SerializeField] PlayerAbilities touch;
    [SerializeField] private Camera cam;

    // Update is called once per frame
    void Update()
    {
        if (touch.initalTouch == false)
        {
            FollowSpeed = 20f;
            yOffset = -1f;
        }

        else
        {
            yOffset = 1f;
        }

        if (cam.orthographicSize <= 10f)
        {
            cam.orthographicSize += 0.005f;
        }

        Vector3 newPos = new Vector3(target.position.x, target.position.y + yOffset, -10f);
        transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.deltaTime);


    }
}
