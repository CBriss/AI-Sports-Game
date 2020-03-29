using System.IO;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Xor : MonoBehaviour
{
  public NeuralNet neuralNet;
  public Graph graph;

  void Update()
  {
    if (Input.GetKey("c"))
    {
      Debug.Log("******************************");
      neuralNet.FeedForward(new float[] { 0, 0 });
      neuralNet.printOutputs();
      neuralNet.FeedForward(new float[] { 0, 1 });
      neuralNet.printOutputs();
      neuralNet.FeedForward(new float[] { 1, 0 });
      neuralNet.printOutputs();
      neuralNet.FeedForward(new float[] { 1, 1 });
      neuralNet.printOutputs();
    }
  }

  // Note: Try convolutional NN?
  public void XorTrain()
  {
    int numberOfInputs = 5000;
    float[,] possibleInputs = new float[,] { { 0, 0 }, { 0, 1 }, { 1, 0 }, { 1, 1 } };
    float[] possibleLabels = new float[] { 0, 1, 1, 0 };
    int batchSize = 16;
    int totalBatchCount = (int)Math.Ceiling((double)numberOfInputs / (double)batchSize);
    Debug.Log("Number of Training Batches: " + totalBatchCount);

    List<float> errors = new List<float>();

    for (int batchNum = 0; batchNum < totalBatchCount - 1; batchNum++)
    {
      Debug.Log("Doing batch: " + batchNum);
      float[][] inputValues = new float[batchSize][];
      float[][] labelValues = new float[batchSize][];

      for (int i = 0; i < batchSize; i++)
      {
        int inputIndex = UnityEngine.Random.Range(0, 3);

        inputValues[i] = new float[] { possibleInputs[inputIndex, 0], possibleInputs[inputIndex, 1] };

        labelValues[i] = new float[] { possibleLabels[inputIndex] };
      }



      // Figure out how to slice arrays
      // Maybe we modify the file I/O to only take it the batchSize number of image/label combos?
      errors.Add(neuralNet.TrainUsingMiniBatch(inputValues, labelValues));
    }
    graph.ShowGraph(errors);
  }
}