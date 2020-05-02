using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new player movement", menuName = "Game Components/Player Movement")]
public class PlayerMovement : MovementController
{
    public KeyCode up_key;
    public KeyCode down_key;
    public KeyCode left_key;
    public KeyCode right_key;
    public float movementSpeed;

    public override void Move(GameObject gameObject)
    {
        Vector3 boatPos = gameObject.transform.position;
        if (Input.GetKey(up_key)) { boatPos.y += movementSpeed * Time.deltaTime; }
        if (Input.GetKey(down_key)) { boatPos.y -= movementSpeed * Time.deltaTime; }
        if (Input.GetKey(left_key)) { boatPos.x -= movementSpeed * Time.deltaTime; }
        if (Input.GetKey(right_key)) { boatPos.x += movementSpeed * Time.deltaTime; }

        Vector3 normalizedBoatPos = Camera.main.WorldToViewportPoint(boatPos);
        normalizedBoatPos.x = Mathf.Clamp01(normalizedBoatPos.x);
        normalizedBoatPos.y = Mathf.Clamp01(normalizedBoatPos.y);
        gameObject.transform.position = Camera.main.ViewportToWorldPoint(normalizedBoatPos);
    }
}
