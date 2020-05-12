using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnChildren : GeneticAlgorithm
{
    void MakeGenerationZero()
    {
        NeuralNet seedBrain;
        seedBrain = new NeuralNet("Assets/Saved Brains/bestBoat.txt");
        for (int i = 0; i < populationSize; i++)
        {
            Vector3 newPlayerNormalizedPosition = Camera.main.ViewportToWorldPoint(
                new Vector3(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.5f), 0)
            );
            newPlayerNormalizedPosition.z = 0;
            game.AddPlayer(newPlayerNormalizedPosition, seedBrain);
        }
    }
}
