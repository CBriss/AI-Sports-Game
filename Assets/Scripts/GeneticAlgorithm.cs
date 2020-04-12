using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm : MonoBehaviour
{
  public int populationSize;
  public GameObject indivualPrefab;
  public List<GameObject> population;
  public GameObject bestIndividual;
  public int generationCount;

  void Start()
  {
    generationCount = 1;
    population = new List<GameObject>();
    MakeGenerationZero();
  }

  void Update()
  {
    int activeObjects = 0;
    for (int i = 0; i < this.population.Count; i++)
    {
        generationCount = 1;
        population = new List<GameObject>();
        MakeGenerationZero();
    }

    if (activeObjects <= 0)
    {
      NewGeneration();
      generationCount += 1;
    }
  }

  void MakeGenerationZero()
  {
    for (int i = 0; i < populationSize; i++)
    {
      this.population.Add(NewIndividual());
    }
  }

  void NewGeneration()
  {
    // Determine Generation Parents
    List<GameObject> parents = DetermineBestParents();

    // Combine parent genes
    // SKIPPING FOR NOW

    // Create new population with those genes and a mutation chance
    for (int i = 0; i < populationSize; i++)
    {
      GameObject individual = NewIndividual();
      individual.GetComponent<Boat>().brain = parents[0].GetComponent<Boat>().brain.Clone();
      this.population.Add(individual);
    }
    this.population = new List<GameObject>();
  }

  private List<GameObject> DetermineBestParents()
  {
    return new List<GameObject>{
            FindBestIndividual(),
            this.population[ UnityEngine.Random.Range(0, this.population.Count)]
        };
  }

    private GameObject FindBestIndividual(){
        GameObject currentBest = this.population[0];
        int currentHighScore = currentBest.GetComponent<Boat>().score;
        foreach (GameObject individual in this.population)
        {
            int currentScore = individual.GetComponent<Boat>().score;
            if(currentScore > currentHighScore){
                currentBest = individual;
                currentHighScore = currentScore;
            }
        }
        return currentBest;
    }

    public GameObject NewIndividual()
    {
        Vector3 newObstacleNormalizedPosition = Camera.main.ViewportToWorldPoint(
            new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 0)
        );
        newObstacleNormalizedPosition.z = 0;

        GameObject individual = ObjectPooling.SharedInstance.GetPooledObject("Boat");
        if (individual != null)
        {
            individual.transform.position = newObstacleNormalizedPosition;
            individual.SetActive(true);
        }

        return individual;
    }

    public GameObject NewIndividual(NeuralNet brain)
    {
      int currentScore = individual.GetComponent<Boat>().score;
      if (currentScore > currentHighScore)
      {
        currentBest = individual;
        currentHighScore = currentScore;
      }
    }
    return currentBest;
  }

  public GameObject NewIndividual()
  {
    Vector3 newObstacleNormalizedPosition = Camera.main.ViewportToWorldPoint(
        new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 0)
    );
    newObstacleNormalizedPosition.z = 0;

    GameObject individual = ObjectPooling.SharedInstance.GetPooledObject("Boat");
    if (individual != null)
    {
      individual.transform.position = newObstacleNormalizedPosition;
      individual.SetActive(true);
    }

    return individual;
  }

}
