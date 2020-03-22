using System.Collections.Generic;
using System;
public partial class NeuralNet
{
  // Set neurons, based on networkShape
  void initNeurons()
  {
    List<float[]> neuronList = new List<float[]>();
    // Add a neuron for every neuron in the layer
    foreach (int layerSize in networkShape) neuronList.Add(new float[layerSize]);
    neurons = neuronList.ToArray();
    preActivatedNeurons = neuronList.ToArray();
  }

  // Set Biases
  // Args can be a set of existing biases to copy
  // If nothing to copy, will set a random value between -0.5 and 0.5
  void initBiases(params float[][] args)
  {
    List<float[]> biasList = new List<float[]>();
    // For each layer
    for (int i = 0; i < networkShape.Length; i++)
    {
      int layerSize = networkShape[i];
      float[] bias = new float[layerSize];
      // Add a random bias for every neuron in the layer
      for (int j = 0; j < layerSize; j++)
      {
        if (args.Length > 0)
          bias[j] = args[i][j];
        else
          bias[j] = UnityEngine.Random.Range(-0.5f, 0.5f);
      }
      biasList.Add(bias);
    }
    biases = biasList.ToArray();
  }

  // Set deltaBiases
  // Makes an array that matches this.biases
  // However, all values initialized to 0
  void initDeltaBiases()
  {
    List<float[]> deltaBiasList = new List<float[]>();
    // For each layer
    foreach (int layerSize in networkShape)
    {
      // Array values default to 0
      deltaBiasList.Add(new float[layerSize]);
    }
    deltaBiases = deltaBiasList.ToArray();
  }

  // Copy weights, from an existing set
  void initWeights(params float[][][] args)
  {
    List<float[][]> weightList = new List<float[][]>();
    // For each non-input layer
    for (int layerIndex = 1; layerIndex < networkShape.Length; layerIndex++)
    {
      List<float[]> weightListForLayer = new List<float[]>();
      int prevLayerNeuronCount = networkShape[layerIndex - 1];
      // For each neuron in layer
      for (int nInd = 0; nInd < neurons[layerIndex].Length; nInd++)
      {
        float[] neuronWeights = new float[prevLayerNeuronCount];
        // For each neuron in previous layer
        for (int prevNInd = 0; prevNInd < prevLayerNeuronCount; prevNInd++)
        {
          if (args.Length > 0)
            neuronWeights[prevNInd] = args[layerIndex][nInd][prevNInd];
          else
            neuronWeights[prevNInd] = UnityEngine.Random.Range(-0.5f, 0.5f);
        }
        weightListForLayer.Add(neuronWeights);
      }
      weightList.Add(weightListForLayer.ToArray());
    }
    weights = weightList.ToArray();
  }

  // Copy weights, from an existing set
  void initDeltaWeights()
  {
    List<float[][]> deltaWeightList = new List<float[][]>();
    // For each layer
    for (int layerIndex = 1; layerIndex < networkShape.Length; layerIndex++)
    {
      List<float[]> deltaWeightListForLayer = new List<float[]>();
      int prevLayerNeuronCount = networkShape[layerIndex - 1];
      // For each neuron in layer
      for (int nInd = 0; nInd < neurons[layerIndex].Length; nInd++)
      {
        float[] neuronDeltaWeights = new float[prevLayerNeuronCount];
        // For each neuron in previous layer that points to current neuron
        for (int prevNInd = 0; prevNInd < prevLayerNeuronCount; prevNInd++)
        {
          neuronDeltaWeights[prevNInd] = 0.0f;
        }
        deltaWeightListForLayer.Add(neuronDeltaWeights);
      }
      deltaWeightList.Add(deltaWeightListForLayer.ToArray());
    }
    deltaWeights = deltaWeightList.ToArray();
  }

}