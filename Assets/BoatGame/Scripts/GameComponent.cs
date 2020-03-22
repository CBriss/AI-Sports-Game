using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameComponent : MonoBehaviour
{
    public float width;
    public float endX;
    public float height;
    public float endY;
    BoxCollider2D componentCollider;

    void Start()
    {
        componentCollider = GetComponent<BoxCollider2D>();
        Vector2 size = componentCollider.bounds.size;
        width = size.x;
        height = size.y;
    }

    void Update()
    {
        endX = transform.position.x + width;
        endY = transform.position.y + height;
    }

    public float GetEndX() { return this.endX; }

    public float GetEndY() { return this.endY; }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "MyGameObjectName") {}

        if (collision.gameObject.tag == "Obstacle")
        {
            gameObject.SetActive(false);
        }
    }
}
