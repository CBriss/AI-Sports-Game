using UnityEngine;

public class BrainTest : PlayerGame
{
    public string seedBoatFileName;

    public void OnEnable()
    {
        LoadBrainButton.OnBrainSelected += SetSeedBoat;
    }

    public void SetSeedBoat(string fileName)
    {
        seedBoatFileName = fileName;
        GetComponentInParent<IGame>().StartGame();
    }

    public override void StartGameController()
    {
        Vector3 newPlayerNormalizedPosition = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0.5f));
        newPlayerNormalizedPosition.z = 0;
        game.AddPlayer(newPlayerNormalizedPosition, new NeuralNet("Assets/Saved Brains/" + seedBoatFileName));
    }

    public void OnDisable()
    {
        LoadBrainButton.OnBrainSelected -= SetSeedBoat;
    }
}
