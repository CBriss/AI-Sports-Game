// public class GradientChecking
// {
//   public NeuralNet neuralNet;
//   public float[] CheckBackpropationXor()
//   {
//     float[,] possibleInputs = new float[,] { { 0, 0 }, { 0, 1 }, { 1, 0 }, { 1, 1 } };
//     float[] possibleLabels = new float[] { 0, 1, 1, 0 };

//     int inputIndex = UnityEngine.Random.Range(0, 3);
//     float[][] inputValues = new float[] { possibleInputs[inputIndex, 0], possibleInputs[inputIndex, 1] };
//     float[] labelValues = new float[] { possibleLabels[inputIndex] };

//     // Manually calculate derivative using an epsilon
//     float[][][] manualdeltaWeights;
//     List<float[][]> manualDeltaWeightsList = new List<float[][]>();
//     for (int layerIndex = 0; layerIndex < neuralNet.weights.Length; layerIndex++)
//     {
//       List<float[][]> manualDeltaWeightsForLayer = new List<float[]>();
//       int prevLayerNeuronCount = networkShape[layerIndex - 1];
//       for (int j = 0; j < neuralNet.weights[layerIndex].Length; j++)
//       {
//         float[] manualDeltaWeightsForNeuron = new float[prevLayerNeuronCount];
//         for (int k = 0; k < neuralNet[layerIndex][j].Length; k++)
//         {
//           // Do Stuff
//         }
//         manualDeltaWeightsForLayer.Add(manualDeltaWeightsForNeuron);
//       }
//       manualDeltaWeightsList.Add(manualDeltaWeightsForLayer.ToArray());
//     }
//     manualdeltaWeights = manualDeltaWeightsList.ToArray();


//     // Feedforward an example through network

//     // For each weight & bias, compare backprop derivative to manual calculation
//     // Average difference for weights & biases
//     // Return both
//   }
// }