using System.Collections.Generic;
using System;
using UnityEngine;
public partial class NeuralNet
{
  public float ReLu(float input)
  {
    return Mathf.Max(0f, input);
  }

  public float ReLuDerivative(float input)
  {
    if (input <= 0.0f)
      return 0;
    else
      return 1;
  }

  public float Sigmoid(float input)
  {
    return 1.0f / (1.0f + Mathf.Exp(-input));
  }

  public float SigmoidDerivative(float input)
  {
    return Sigmoid(input) * (1 - Sigmoid(input));
  }

  public float[] SoftMax(float[] inputs)
  {
    if (inputs.Length <= 1)
      return inputs;
    float denominator = 0;
    for (int i = 0; i < inputs.Length; i++)
    {
      denominator += Mathf.Exp(inputs[i]);
    }

    float[] softmax = new float[inputs.Length];
    for (int i = 0; i < inputs.Length; i++)
    {
      softmax[i] = (Mathf.Exp(inputs[i]) / denominator);
    }
    // Debug.Log("Softmax sum was: " + denominator + " and inputs are: " + String.Join("|", inputs));
    return softmax;
  }

}