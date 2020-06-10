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
        game.AddPlayer(new NeuralNet("Assets/Saved Brains/" + seedBoatFileName));
    }

    public void OnDisable()
    {
        LoadBrainButton.OnBrainSelected -= SetSeedBoat;
    }
}
