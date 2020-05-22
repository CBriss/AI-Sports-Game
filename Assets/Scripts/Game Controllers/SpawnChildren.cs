using System.IO;
using UnityEngine;

public class SpawnChildren : GeneticAlgorithm
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

    public new void MakeGenerationZero()
    {
        NeuralNet seedBrain;
        seedBrain = new NeuralNet("Assets/Saved Brains/" + seedBoatFileName);
        for (int i = 0; i < populationSize; i++)
        {
            Vector3 newPlayerNormalizedPosition = Camera.main.ViewportToWorldPoint(
                new Vector3(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.5f), 0)
            );
            newPlayerNormalizedPosition.z = 0;
            game.AddPlayer(newPlayerNormalizedPosition, seedBrain);
        }
    }

    public void OnDisable()
    {
        LoadBrainButton.OnBrainSelected -= SetSeedBoat;
    }
}
