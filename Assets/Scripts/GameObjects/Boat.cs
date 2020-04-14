using UnityEngine;

public class Boat : GameComponent
{
    private Vector2 bounds;
    public int score;
    public float distanceTraveled;

    public NeuralNet brain;

    /*
    void Start()
    {
        gameSpeed = GameObject.Find("BoatGameWrapper").GetComponent<BoatGame>().gameSpeed;
        distanceTraveled = 0;
    }

    void Update()
    {
        UpdateScore();
    }

    void UpdateScore()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        float heightOnScreen = pos.y;
        float newDistanceTraveled = gameSpeed;
        score += Mathf.RoundToInt(Mathf.Ceil((newDistanceTraveled) * (heightOnScreen * 1.5f)));
        distanceTraveled += newDistanceTraveled;
    }
    */
}
