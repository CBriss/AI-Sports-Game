using System.IO;
using System;
using UnityEngine;

/*
0000     32 bit integer  0x00000801(2049) magic number (MSB first)
0004     32 bit integer  60000            number of items
0008     unsigned byte   ??               label
0009     unsigned byte   ??               label
........
xxxx     unsigned byte   ??               label
*/
public class LabelFile
{
  public string fileName = "";
  public int numberOfLabels;
  public float[][] labels;

  public LabelFile(string inputFileName)
  {
    fileName = inputFileName;
    FindHeaders(fileName);
  }

  private void FindHeaders(string fileName)
  {
    // open file
    // read & set headers
    using (FileStream inputStream = File.OpenRead(fileName))
    {
      byte[] buffer = new byte[4];
      var bytesRead = inputStream.Read(buffer, 0, 4);
      var magicNumber = SwapEndianness(BitConverter.ToInt32(buffer, 0));
      Debug.Log($"Magic number: {magicNumber}");

      bytesRead = inputStream.Read(buffer, 0, 4);
      numberOfLabels = SwapEndianness(BitConverter.ToInt32(buffer, 0));
      Debug.Log($"Number of Labels: {numberOfLabels}");
    }
  }

  // Reads batch of labels
  // Note: This expects the MNIST labelset
  public void ReadNextBatch(int startPoint, int endPoint)
  {
    using (FileStream fileStream = File.OpenRead(fileName))
    {
      // Set pointer to first label
      fileStream.Seek(16 + (startPoint), SeekOrigin.Begin);

      int labelCount = endPoint - startPoint;
      labels = new float[labelCount][];
      for (int i = 0; i < labelCount; i++)
      {
        byte singleBuffer;
        singleBuffer = (byte)fileStream.ReadByte();
        float[] outputRow = new float[10];
        // So we get the value, and set it to 1

        outputRow[(int)singleBuffer] = 1.0f;
        labels[i] = outputRow;
      }
    }
  }

  private static int SwapEndianness(int value)
  {
    var b1 = (value >> 0) & 0xff;
    var b2 = (value >> 8) & 0xff;
    var b3 = (value >> 16) & 0xff;
    var b4 = (value >> 24) & 0xff;

    return b1 << 24 | b2 << 16 | b3 << 8 | b4 << 0;
  }
}