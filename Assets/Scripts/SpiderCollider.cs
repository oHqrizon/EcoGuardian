using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpiderCollider : MonoBehaviour
{
    private SpriteRenderer spriteRend;
    private PolygonCollider2D polyCollider;

    // Start is called before the first frame update
    void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        polyCollider = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2[] spriteVertices = spriteRend.sprite.vertices;

        polyCollider.SetPath(0, spriteVertices);
    }
}
