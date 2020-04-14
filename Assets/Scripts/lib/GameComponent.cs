using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameComponent : MonoBehaviour
{
    public GameComponentTemplate template;
    public float endX;
    public float endY;
    BoxCollider2D collider;
    private NeuralNet brain;

    void Start()
    {

        //Set Size
        gameObject.GetComponent<RectTransform>().sizeDelta = template.colliderSize;

        // Create Collider
        BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
        collider.size = template.colliderSize;
        collider.isTrigger = true;

        // Set image
        gameObject.GetComponent<SpriteRenderer>().sprite = template.image;
        gameObject.GetComponent<SpriteRenderer>().transform.localScale = template.imageSize;

        // Create brain, if needed
        if (template.hasBrain)
            brain = new NeuralNet(new int[] { 2, 20, 4 });

        // Add Movement Script
        //gameObject.AddComponent(IMovab template.movable);
    }

    GameObject FindNearest(GameObject[] objects)
    {
        GameObject closestObject = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject gameObject in objects)
        {
            float newDistance = (gameObject.transform.position - position).sqrMagnitude;
            if (newDistance < distance)
            {
                closestObject = gameObject;
                distance = newDistance;
            }
        }
        return closestObject;
    }

    void Update()
    {
        endX = transform.position.x + template.colliderSize.x;
        endY = transform.position.y + template.colliderSize.y;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            gameObject.SetActive(false);
        }
    }
}