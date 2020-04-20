using UnityEngine;

public class GameComponent : MonoBehaviour
{
    public GameComponentTemplate template;
    public float endX;
    public float endY;
    public NeuralNet brain;

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
        if(template.movementAbility)
            template.movementAbility.Move(gameObject);
        endX = transform.position.x + template.colliderSize.x;
        endY = transform.position.y + template.colliderSize.y;
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.tag == "Player" && collision.gameObject.tag == "Obstacle")
        {
            gameObject.SetActive(false);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}