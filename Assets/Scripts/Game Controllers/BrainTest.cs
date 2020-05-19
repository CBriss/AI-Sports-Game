using UnityEngine;

public class BrainTest : PlayerGame
{
    public override void StartGameController()
    {
        Vector3 newPlayerNormalizedPosition = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0.5f));
        newPlayerNormalizedPosition.z = 0;
        game.AddPlayer(newPlayerNormalizedPosition, new NeuralNet("Assets/Saved Brains/bestBoat.txt"));
    }
}
