using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
public partial class NeuralNet
{
  public void SaveToFile()
  {
    try
    {
      using (StreamWriter writeStream = new StreamWriter(new FileStream("Assets/Scripts/test.txt", FileMode.OpenOrCreate, FileAccess.Write)))
      {
        writeStream.WriteLine(string.Join(",", networkShape));

        writeStream.WriteLine("Biases");
        for (int i = 0; i < networkShape.Length; i++)
        {
          writeStream.WriteLine("Layer " + i);
          writeStream.WriteLine(string.Join(",", biases[i]));
        }

        writeStream.WriteLine("Weights");
        for (int i = 0; i < weights.Length; i++)
        {
          writeStream.WriteLine("Layer " + i);
          for (int j = 0; j < weights[i].Length; j++)
          {
            writeStream.WriteLine("Destination Neuron Index " + j);
            writeStream.WriteLine(string.Join(",", weights[i][j]));
          }

        }
      }
    }
    catch (IOException e)
    {
      Console.WriteLine("The file could not be read:");
      Console.WriteLine(e.Message);
    }


  }

  public void readBiasesFromFile(StreamReader readStream)
  {
    while (true)
    {
      int currentLayer = 0;
      string fileLine = readStream.ReadLine();
      if (fileLine.Equals("Weights"))
        break;
      if (fileLine.Contains("Layer"))
      {
        currentLayer = int.Parse(fileLine.Split(' ')[1]);
      }
      else
      {
        string[] inputBiases = fileLine.Split(',');
        for (int i = 0; i < biases[currentLayer].Length; i++)
        {
          biases[currentLayer][i] = int.Parse(inputBiases[i]);
        }
      }
    }
  }

  public void readWeightsFromFile(StreamReader readStream)
  {
    while (readStream.Peek() >= 0)
    {
      int currentLayer = 0;
      int currentNeuronIndex = 0;
      string fileLine = readStream.ReadLine();
      string[] line = fileLine.Split(' ');
      if (fileLine.Contains("Layer"))
      {
        currentLayer = int.Parse(line[1]);
      }
      else if (fileLine.Contains("Destination Neuron Index"))
      {
        currentNeuronIndex = int.Parse(line[line.Length]);
      }
      else
      {
        string[] inputWeights = fileLine.Split(',');
        for (int i = 0; i < weights[currentLayer][currentNeuronIndex].Length; i++)
        {
          weights[currentLayer][currentNeuronIndex][i] = int.Parse(inputWeights[i]);
        }
      }
    }
  }
}
