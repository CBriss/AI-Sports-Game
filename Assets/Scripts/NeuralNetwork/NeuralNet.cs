using System;
using System.IO;
using UnityEngine;

using static DNA;

public partial class NeuralNet
{
  public int[] networkShape;
  public float[][] neurons;
  public float[][] preActivatedNeurons;
  public float[][] biases;
  public float[][][] weights;
  public float[][] deltaBiases;
  public float[][][] deltaWeights;

  public string activationFunction;
  public float learningRate;

    /*
    ##################
    # Initialization #
    ##################
    */
    // Constructor
    private void Start()
    {
        initNeurons();
        initBiases();
        initWeights();
    }

    public NeuralNet(int[] networkShape)
    {
        this.networkShape = networkShape;
        initNeurons();
        initBiases();
        initWeights();
    }

    public NeuralNet(string filePath)
    {
        try
        {
        using (StreamReader readStream = new StreamReader(filePath))
        {
            string[] shape = readStream.ReadLine().Split(',');
            for (int i = 0; i < shape.Length; i++)
            {
            networkShape[i] = int.Parse(shape[i]);
            }

            if (readStream.ReadLine().Equals("Biases"))
            {
            readBiasesFromFile(readStream);
            }
            readWeightsFromFile(readStream);
        }
        }
        catch (IOException e)
        {
        Console.WriteLine("The file could not be read:");
        Console.WriteLine(e.Message);
        }
    }

  /*
  ################
  # Network Input/Output #
  ################
  */
  public void FeedForward(float[] inputs)
  {
    // Set input layer to inputs
    for (int i = 0; i < inputs.Length; i++)
    {
      neurons[0][i] = inputs[i];
    }

    // For each non-input layer
    for (int layer = 1; layer < networkShape.Length; layer++)
    {
      // For each neuron in layer
      for (int neuron = 0; neuron < neurons[layer].Length; neuron++)
      {
        float z = 0f;
        // Calculate neuron value
        // by adding all previous neurons * weights
        for (int prevNeuronIndx = 0; prevNeuronIndx < neurons[layer - 1].Length; prevNeuronIndx++)
        {
          z += weights[layer - 1][neuron][prevNeuronIndx] * neurons[layer - 1][prevNeuronIndx];
        }
        z += biases[layer][neuron];
        // Saving value before activation (for z in backpropagation)
        preActivatedNeurons[layer][neuron] = z;
        // Activation function on neuron
        neurons[layer][neuron] = Activate(activationFunction, z);
      }
    }
  }

  public float TrainUsingMiniBatch(float[][] inputs, float[][] expected_outputs)
  {
    // Initialize delta totals
    initDeltaBiases();
    initDeltaWeights();

    // For each step in batch
    int batch_size = inputs.Length;
    for (int stepCount = 0; stepCount < inputs.Length; stepCount++)
    {
      BackpropogateTrainingExample(inputs[stepCount], expected_outputs[stepCount], this.networkShape.Length - 1);
    }

    // Calculate error, only used to print stuff
    float mse = 0.0f;
    for (int neuronIndx = 0; neuronIndx < neurons[networkShape.Length - 1].Length; neuronIndx++)
    {
      mse += (
        (neurons[networkShape.Length - 1][neuronIndx] - expected_outputs[inputs.Length - 1][neuronIndx]) *
        (neurons[networkShape.Length - 1][neuronIndx] - expected_outputs[inputs.Length - 1][neuronIndx])
      );
    }
    mse = mse / neurons[networkShape.Length - 1].Length;
    Debug.Log("Error: " + mse);


    // Adjust weights and biases by deltas
    for (int i = 0; i < biases.Length; i++)
    {
      for (int j = 0; j < biases[i].Length; j++)
      {
        if (deltaBiases[i][j] != 0)
        {
          biases[i][j] -= (deltaBiases[i][j] / batch_size) * learningRate;
        }
      }
    }

    // Average deltas by dividing by inputs.Length first
    for (int i = 0; i < weights.Length; i++)
    {
      for (int j = 0; j < weights[i].Length; j++)
      {
        for (int k = 0; k < weights[i][j].Length; k++)
        {
          if (deltaWeights[i][j][k] != 0)
          {
            weights[i][j][k] -= (deltaWeights[i][j][k] / batch_size) * learningRate;
          }
        }
      }
    }

    return mse;

  }

  void BackpropogateTrainingExample(float[] inputs, float[] expected_outputs, int outputLayer)
  {

    // Process input
    FeedForward(inputs);
    // float derivativeMSE = 0.0f;
    // for (int neuronIndx = 0; neuronIndx < neurons[outputLayer].Length; neuronIndx++)
    //   derivativeMSE += (neurons[outputLayer][neuronIndx] - expected_outputs[neuronIndx]);

    // derivativeMSE = derivativeMSE * 2;
    // For each ouput neuron
    for (int neuronIndx = 0; neuronIndx < neurons[outputLayer].Length; neuronIndx++)
    {
      // Adjusting Output Biases
      // Also the local gradient
      deltaBiases[outputLayer][neuronIndx] += (neurons[outputLayer][neuronIndx] - expected_outputs[neuronIndx]) * Activate(activationFunction, preActivatedNeurons[outputLayer][neuronIndx]);

      // Adjusting Output Weights
      int previousLayer = outputLayer - 1;
      // For each neuron in previous layer
      for (int prevNeuronIndx = 0; prevNeuronIndx < neurons[previousLayer].Length; prevNeuronIndx++)
      {
        deltaWeights[previousLayer][neuronIndx][prevNeuronIndx] += deltaBiases[outputLayer][neuronIndx] * neurons[previousLayer][prevNeuronIndx];
      }
    }

    // Propogate backwards, excluding input layer
    for (int layer = outputLayer - 1; layer > 0; layer--)
    {
      // For each neuron in layer
      for (int neuronIndx = 0; neuronIndx < neurons[layer].Length; neuronIndx++)
      {
        // Adjust Biases
        // For each neuron in layer + 1
        for (int nextNeuronIndx = 0; nextNeuronIndx < neurons[layer + 1].Length; nextNeuronIndx++)
        {
          deltaBiases[layer][neuronIndx] += weights[layer][nextNeuronIndx][neuronIndx] * deltaBiases[layer + 1][nextNeuronIndx];
        }
        deltaBiases[layer][neuronIndx] *= SigmoidDerivative(preActivatedNeurons[layer][neuronIndx]);

        // Adjust Weights
        // For each neuron in layer - 1
        for (int prevNeuronIndx = 0; prevNeuronIndx < neurons[layer - 1].Length; prevNeuronIndx++)
        {
          deltaWeights[layer - 1][neuronIndx][prevNeuronIndx] += deltaBiases[layer][neuronIndx] * neurons[layer - 1][prevNeuronIndx];
        }
      }
    }
  }

  /*
  ############
  # Misc #
  ############
  */
  public float[] Outputs()
  {
    return neurons[networkShape.Length - 1];
  }

    public NeuralNet Clone()
    {
        NeuralNet copy = new NeuralNet(this.networkShape);
        // Copy network shape
        copy.networkShape = this.networkShape;

        // Copy neurons
        copy.neurons = this.neurons;

        // Copy biaees
        copy.biases = this.biases;

        // Copy weights
        copy.weights = this.weights;

        // Copy Learning Rate
        copy.learningRate = this.learningRate;

        // Mutate
        copy = Mutate(copy, 0.25f);

        return copy;
    }


    public NeuralNet Mutate(NeuralNet neuralNet, float goalPercentage) {
        // for each layer
        for (int i = 0; i < neuralNet.weights.Length; i++)
        {
            for (int j = 0; j < neuralNet.weights[i].Length; j++)
            {
                for (int k = 0; k < neuralNet.weights[i][j].Length; k++)
                {
                    neuralNet.weights[i][j][k] = MutateWeight(neuralNet.weights[i][j][k], goalPercentage);
                }
            }
        }
        return neuralNet;
    }

    private float MutateWeight(float weight, float goalPercentage)
    {
        if (UnityEngine.Random.Range(0.0f, 1.0f) < (0.10 * (1 - goalPercentage)))
        {
            return weight + UnityEngine.Random.Range(0.0f, 1.0f) * 0.5f;
        }
        return weight;
    }
}