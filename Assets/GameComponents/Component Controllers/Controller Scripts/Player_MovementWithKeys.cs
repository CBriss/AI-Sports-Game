using UnityEngine;

[CreateAssetMenu(fileName = "new player keys movement", menuName = "Game Components/Movement/Player Keys Movement")]
public class Player_MovementWithKeys : ComponentController
{
    [SerializeField]
    private KeyCode up_key;
    [SerializeField]
    private KeyCode down_key;
    [SerializeField]
    private KeyCode left_key;
    [SerializeField]
    private KeyCode right_key;
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private bool clampToScreen;

    public override void UpdateComponent(GameComponent gameComponent)
    {
        Move(gameComponent);
    }
    public void Move(GameComponent gameComponent)
    {
        Vector2 objectPosition = gameComponent.transform.position;
        if (Input.GetKey(up_key)) { objectPosition.y += movementSpeed * Time.deltaTime; }
        if (Input.GetKey(down_key)) { objectPosition.y -= movementSpeed * Time.deltaTime; }
        if (Input.GetKey(left_key)) { objectPosition.x -= movementSpeed * Time.deltaTime; }
        if (Input.GetKey(right_key)) { objectPosition.x += movementSpeed * Time.deltaTime; }

        gameComponent.SetPosition(objectPosition, clampToScreen);
    }
}
