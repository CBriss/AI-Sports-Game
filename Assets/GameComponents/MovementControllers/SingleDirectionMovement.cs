using UnityEngine;

[CreateAssetMenu(fileName = "new single direction movement", menuName = "Game Components/Single Direction Movement")]
public class SingleDirectionMovement : MovementController
{
    public float movementSpeed;
    public string direction;

    public override void Move(GameObject gameObject)
    {
        Vector3 pos = gameObject.transform.position;

        switch (direction)
        {
            case "up":
                pos.y += movementSpeed * Time.deltaTime;
                break;
            case "down":
                pos.y -= movementSpeed * Time.deltaTime;
                break;
            case "left":
                pos.x += movementSpeed * Time.deltaTime;
                break;
            case "right":
                pos.x += movementSpeed * Time.deltaTime;
                break;
        }
        gameObject.transform.position = pos;
    }
}
