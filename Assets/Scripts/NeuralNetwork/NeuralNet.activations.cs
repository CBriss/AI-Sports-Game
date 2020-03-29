using System.Collections.Generic;
using System;
using UnityEngine;
public partial class NeuralNet
{

  public float Activate(string functionName, float input){
    switch(functionName)
    {
      case "relu":
        return ReLu(input);
      case "leakyrelu":
        return LeakyReLu(input);
      case "tanh":
        return Tanh(input);
      case "sigmoid":
        return Sigmoid(input);
      default:
        return Sigmoid(input);
    }
  }

  public float DerivativeActivate(string functionName, float input){
    switch(functionName)
    {
      case "relu":
        return ReLuDerivative(input);
      case "leakyrelu":
        return LeakyReLuDerivative(input);
      case "tanh":
        return TanhDerivative(input);
      case "sigmoid":
        return SigmoidDerivative(input);
      default:
        return SigmoidDerivative(input);
    }
  }
  
  private float ReLu(float input)
  {
    return Mathf.Max(0f, input);
  }

  private float ReLuDerivative(float input)
  {
    if (input <= 0.0f)
      return 0;
    else
      return 1;
  }

   private float LeakyReLu(float input)
  {
    if(input <= 0.0f)
      return 0.01f * input;
    else
      return input;
  }

  private float LeakyReLuDerivative(float input)
  {
    if(input <= 0.0f)
      return 0.01f;
    else
      return 1;
  }

  private float Tanh(float input)
  {
    return (float)Math.Tanh(input);
  }

  private float TanhDerivative(float input)
  {
      return 1 - (input * input);
  }
 
  private float Sigmoid(float input)
  {
    return 1.0f / (1.0f + Mathf.Exp(-input));
  }

  private float SigmoidDerivative(float input)
  {
    return Sigmoid(input) * (1 - Sigmoid(input));
  }

  public float[] SoftMax(float[] inputs)
  {
    if (inputs.Length <= 1)
      return inputs;
    float denominator = 0.0f;
    for (int i = 0; i < inputs.Length; i++)
    {
      denominator += inputs[i];
    }

    float[] softmax = new float[inputs.Length];
    for (int i = 0; i < inputs.Length; i++)
    {
      // softmax[i] = (Mathf.Exp(inputs[i]) / denominator);
      softmax[i] = inputs[i] / denominator;
    }
    Debug.Log("Softmax sum was: " + denominator + " and inputs are: " + String.Join("|", inputs));
    return softmax;
  }

}