using UnityEngine;

[CreateAssetMenu(fileName = "new single direction", menuName = "Game Components/Movement/AI Single Direction")]
public class AI_SingleDirection : ComponentController
{
    public float movementSpeed;
    public Vector2 direction;

    public override void UpdateComponent(GameComponent gameComponent)
    {
        Move(gameComponent);
    }
    public void Move(GameComponent gameComponent)
    {
        Vector2 pos = gameComponent.transform.position;
        gameComponent.transform.position = pos + direction * movementSpeed * Time.deltaTime;
    }
}
