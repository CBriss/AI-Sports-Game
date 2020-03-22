using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    private float gameSpeed;
    private Vector2 bounds;
    public int score;
    public float distanceTraveled;
    GameComponent component;

    void Start()
    {
        gameSpeed = GameObject.Find("BoatGameWrapper").GetComponent<BoatGame>().gameSpeed;
        component = GetComponent<GameComponent>();
        distanceTraveled = 0;
    }

    void Update()
    {
        MoveFromLearning();
        UpdateScore();
    }

    void MoveFromInput()
    {
        Vector3 boatPos = transform.position;
        if (Input.GetKey("w")) { boatPos.y += gameSpeed * Time.deltaTime; }
        if (Input.GetKey("s")) { boatPos.y -= gameSpeed * Time.deltaTime; }
        if (Input.GetKey("d")) { boatPos.x += gameSpeed * Time.deltaTime; }
        if (Input.GetKey("a")) { boatPos.x -= gameSpeed * Time.deltaTime; }
        transform.position = boatPos; 
    }

    void MoveFromLearning()
    {
        Vector3 boatPos = transform.position;
        float randomNumber = Random.Range(0f, 1f);
        if (randomNumber < 0.25f) { boatPos.y += gameSpeed * Time.deltaTime; }
        else if (randomNumber < 0.5f) { boatPos.y -= gameSpeed * Time.deltaTime; }
        else if (randomNumber < 0.75f) { boatPos.x += gameSpeed * Time.deltaTime; }
        else { boatPos.x -= gameSpeed * Time.deltaTime; }
        transform.position = boatPos;
    }

    void UpdateScore()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        float heightOnScreen = pos.y;
        float newDistanceTraveled = gameSpeed;
        score += Mathf.RoundToInt(Mathf.Ceil((newDistanceTraveled) * (heightOnScreen * 1.5f)));
        distanceTraveled += newDistanceTraveled;
    }

    void LateUpdate()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }
}
