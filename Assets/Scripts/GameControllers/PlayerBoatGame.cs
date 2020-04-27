using UnityEngine;

public class PlayerBoatGame : GameController
{
    public IGame game;
    
    public override void Start()
    {
        Vector3 newPlayerNormalizedPosition = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0.5f));
        newPlayerNormalizedPosition.z = 0;
        game = gameObject.GetComponent<IGame>();
        game.AddPlayer(newPlayerNormalizedPosition);
    }

    public override void Update()
    {
    }
}
