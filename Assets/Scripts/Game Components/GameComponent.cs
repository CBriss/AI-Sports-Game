using System;
using UnityEngine;

public class GameComponent : MonoBehaviour
{
    public GameComponentTemplate template;
    public NeuralNet brain;

    private Camera camera;

    public static event Action<GameObject, GameObject> OnComponentCollision = delegate { };

    private void Awake()
    {
        camera = Camera.main;
    }
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
        Vector3 normalizedBoatPos = camera.WorldToViewportPoint(newPosition);
        if (clamptoScreen)
        {
            normalizedBoatPos.x = Mathf.Clamp01(normalizedBoatPos.x);
            normalizedBoatPos.y = Mathf.Clamp01(normalizedBoatPos.y);
        }
        gameObject.transform.position = camera.ViewportToWorldPoint(normalizedBoatPos);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        OnComponentCollision(gameObject, collision.gameObject);
    }

    void OnBecameInvisible()
    {
        if (gameObject.tag == "Obstacle")
            gameObject.SetActive(false);
    }
}