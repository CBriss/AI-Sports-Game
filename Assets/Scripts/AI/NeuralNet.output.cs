using UnityEngine;

public partial class NeuralNet
{

  /*
  ################
  # Printing #
  ################
  */
  public void printWeights(int startingLayer)
  {
    string weightString = "Weights:\n";
    for (int i = startingLayer; i < weights.Length; i++)
    {
      weightString += "Layer " + i + "\n";
      for (int j = 0; j < weights[i].Length; j++)
      {
        for (int k = 0; k < weights[i][j].Length; k++)
        {
          if (weights[i][j][k] != 0)
            weightString += weights[i][j][k] + "|";
        }
        weightString += "\n";
      }
    }
    Debug.Log(weightString);
  }

  public void printBiases()
  {
    string biasString = "Biases:\n";
    for (int i = 0; i < biases.Length; i++)
    {
      biasString += "\nLayer " + i + "\n";
      for (int j = 0; j < biases[i].Length; j++)
      {
        if (biases[i][j] != 0)
          biasString += biases[i][j] + "|";
      }
    }
    Debug.Log(biasString);
  }

  public void printOutputs()
  {
    string outputs = "";
    for (int i = 0; i < networkShape[networkShape.Length - 1]; i++)
    {
      outputs += neurons[networkShape.Length - 1][i] + ", ";
    }
    Debug.Log(outputs);
  }

  public void printDeltaWeights(int startingLayer)
  {
    string deltaWeightString = "DeltaWeights:\n";
    for (int i = startingLayer; i < deltaWeights.Length; i++)
    {
      deltaWeightString += "\nLayer " + i + "\n";
      for (int j = 0; j < deltaWeights[i].Length; j++)
      {
        for (int k = 0; k < deltaWeights[i][j].Length; k++)
        {
          if (deltaWeights[i][j][k] != 0)
            deltaWeightString += deltaWeights[i][j][k] + "|";
        }
      }
    }
    Debug.Log(deltaWeightString);
  }

  public void printDeltaBiases()
  {
    string deltaBiasString = "DeltaBiases:\n";
    for (int i = 0; i < deltaBiases.Length; i++)
    {
      deltaBiasString += "\nLayer " + i + "\n";
      for (int j = 0; j < deltaBiases[i].Length; j++)
      {
        if (deltaBiases[i][j] != 0)
          deltaBiasString += deltaBiases[i][j] + "|";
      }
    }
    Debug.Log(deltaBiasString);
  }

}