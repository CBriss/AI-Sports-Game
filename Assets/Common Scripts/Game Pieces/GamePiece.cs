using System;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
    public GamePieceTemplate template;
    public GamePieceMovement movement;
    public NeuralNet brain;

    public Camera mainCamera;

    public Player player;

    public static event Action<GameObject, GameObject> OnComponentCollision = delegate { };

    private void Awake()
    {
        mainCamera = Camera.main;
        movement = gameObject.GetComponent<GamePieceMovement>();
    }

    void Start()
    {
        //Set Size
        gameObject.GetComponent<RectTransform>().sizeDelta = template.size;

        // Set image
        gameObject.GetComponent<SpriteRenderer>().sprite = template.image;
        gameObject.GetComponent<SpriteRenderer>().transform.localScale = template.imageSize;

        // Create Collider
        if (template.useComplexCollider)
        {
            PolygonCollider2D collider = gameObject.AddComponent<PolygonCollider2D>();
            if (!template.hasPhysics)
                collider.isTrigger = true;
        }
        else
        {
            BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
            collider.size = template.size;
            if (!template.hasPhysics)
                collider.isTrigger = true;
        }

        // Set RigidBody to Dynamic if movable by physics
        if (template.movableByPhysics)
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        // Set Tag
        gameObject.tag = template.tagName;

        // Set Brain, If AI
        if (template.hasBrain && template.brainShape != null)
            brain = new NeuralNet(template.brainShape);
    }

    void Update()
    {
        template.componentController.UpdateComponent(this);
    }

    public void SetPosition(Vector2 newPosition, bool clamptoScreen)
    {
        Vector3 normalizedBoatPos = mainCamera.WorldToViewportPoint(newPosition);
        if (clamptoScreen)
        {
            normalizedBoatPos.x = Mathf.Clamp01(normalizedBoatPos.x);
            normalizedBoatPos.y = Mathf.Clamp01(normalizedBoatPos.y);
        }
        gameObject.transform.position = mainCamera.ViewportToWorldPoint(normalizedBoatPos);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        OnComponentCollision(gameObject, collision.gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        OnComponentCollision(gameObject, collision.gameObject);
    }

    void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}