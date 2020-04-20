using System.IO;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Mnist : MonoBehaviour
{
  public NeuralNet neuralNet;
  public GameObject blackSquare;
  public Graph graph;

  int[,] squares = new int[28, 28];

  float gridSquareWidth;
  float gridSquareHeight;

  void Start()
  {
    // Split part of screen into 28x28 blocks
    gridSquareWidth = (float)Screen.width / 28;
    gridSquareHeight = (float)Screen.width / 28;
  }

  void Update()
  {
    // Drawing
    if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetMouseButton(0))
    {
      Draw();
    }
    // Feed drawing through neural net
    else if (Input.GetKey("p"))
    {
      float[] inputs = new float[28 * 28];
      int inputsIndex = 0;
      for (int i = 0; i < 27; i++)
      {
        for (int j = 0; j < 28; j++)
        {
          if (squares[i, j] >= 1)
            inputs[inputsIndex] = 1;
          else
            inputs[inputsIndex] = 0;
          inputsIndex += 1;
        }
        inputsIndex += 1;
      }
      neuralNet.FeedForward(inputs);
      Debug.Log(ParseNetworkOutput(neuralNet.Outputs()));
    }
    else if (Input.GetKey("t"))
    {
      MnistTrain();
    }
  }

  void Draw()
  {
    int[] gridPos = new int[2];
    gridPos[0] = (int)Mathf.Floor(Input.mousePosition.x / gridSquareWidth); // x position
    gridPos[1] = (int)Mathf.Floor(Input.mousePosition.y / gridSquareHeight); // y position

    if (squares[gridPos[0], gridPos[1]] != 1)
    {
      GameObject newSquare = Instantiate(blackSquare);
      Vector3 newSquarePos = Camera.main.ScreenToWorldPoint(
          new Vector3((float)(gridPos[0] * gridSquareWidth), (float)(gridPos[1] * gridSquareHeight), 0)
      );
      newSquarePos.z = 0;
      newSquare.transform.position = newSquarePos;

      squares[gridPos[0], gridPos[1]] = 1;
    }
  }

  public void ResetSquares()
  {

    GameObject[] blackSquares = GameObject.FindGameObjectsWithTag("BlackSquare");
    foreach (GameObject blackSquare in blackSquares)
    {
      Destroy(blackSquare);
    }
    squares = new int[28, 28];
  }
  public float ParseNetworkOutput(float[] outputs)
  {
    // Debug.Log("Outputs Given: " + String.Join(",", outputs));
    float currentMax = 0;
    float currentMaxIndex = 0;
    for (int i = 0; i < outputs.Length; i++)
    {
      if (outputs[i] > currentMax)
      {
        currentMax = outputs[i];
        currentMaxIndex = i;
      }
    }
    return currentMaxIndex;
  }

  // Note: Try convolutional NN?
  public void MnistTrain()
  {
    string TrainImages = "Assets/mnist/train/train-images.idx3-ubyte";
    string TrainLabels = "Assets/mnist/train/train-labels.idx1-ubyte";
    var trainingImageFile = new ImageFile(TrainImages); // Training Images
    var trainingLabelFile = new LabelFile(TrainLabels); // Training Labels

    int batchSize = 8;
    int totalBatchCount = (int)Math.Ceiling((double)trainingLabelFile.numberOfLabels / (double)batchSize);
    Debug.Log("Number of Training Batches: " + totalBatchCount);

    List<float> errors = new List<float>();

    for (int batchNum = 0; batchNum < 100; batchNum++)
    {
      // Debug.Log("Doing batch: " + batchNum);
      int startPoint = batchNum * batchSize;
      int endPoint = startPoint + batchSize - 1;


      if (endPoint >= trainingLabelFile.numberOfLabels)
        endPoint = trainingLabelFile.numberOfLabels - 1;
      trainingImageFile.ReadNextBatch(startPoint, endPoint);
      trainingLabelFile.ReadNextBatch(startPoint, endPoint);

      // Figure out how to slice arrays
      // Maybe we modify the file I/O to only take it the batchSize number of image/label combos?
      errors.Add(neuralNet.TrainUsingMiniBatch(trainingImageFile.images, trainingLabelFile.labels));
      // neuralNet.printOutputs();
    }
    graph.ShowGraph(errors);
    // MnistTest();
  }

  public void MnistTest()
  {
    string TestImages = "Assets/BoatGame/mnist/test/t10k-images.idx3-ubyte";
    string TestLabels = "Assets/BoatGame/mnist/test/t10k-labels.idx1-ubyte";
    var testingImageFile = new ImageFile(TestImages); // Testing Images
    var testingLabelFile = new LabelFile(TestLabels); // Testing Labels

    int correctGuessCount = 0;
    for (int batchNum = 0; batchNum < 100; batchNum++)
    {
      int startPoint = batchNum;
      int endPoint = startPoint + 1;

      testingImageFile.ReadNextBatch(startPoint, endPoint);
      testingLabelFile.ReadNextBatch(startPoint, endPoint);

      neuralNet.FeedForward(testingImageFile.images[0]);
      Debug.Log((int)ParseNetworkOutput(neuralNet.Outputs()));
      Debug.Log(String.Join(",", testingLabelFile.labels[0]));
      if (testingLabelFile.labels[0][(int)ParseNetworkOutput(neuralNet.Outputs())] > 0.0f)
        correctGuessCount += 1;
    }

    Debug.Log((correctGuessCount / TestImages.Length) * 100);
  }
}