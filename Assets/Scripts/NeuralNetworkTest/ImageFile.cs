using System.IO;
using System;
using UnityEngine;

/*
    [offset] [type]          [value]          [description]
    0000     32 bit integer  0x00000803(2051) magic number
    0004     32 bit integer  60000            number of images
    0008     32 bit integer  28               number of rows
    0012     32 bit integer  28               number of columns
    0016     unsigned byte   ??               pixel
    0017     unsigned byte   ??               pixel
    ........
    xxxx     unsigned byte   ??               pixel
    */
public class ImageFile
{
  public string fileName = "";
  public int numberOfImages;
  public int numberOfRows;
  public int numberOfColumns;
  public float[][] images;

  public ImageFile(string inputFileName)
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
      numberOfImages = SwapEndianness(BitConverter.ToInt32(buffer, 0));
      Debug.Log($"Number of images: {numberOfImages}");

      bytesRead = inputStream.Read(buffer, 0, 4);
      numberOfRows = SwapEndianness(BitConverter.ToInt32(buffer, 0));

      bytesRead = inputStream.Read(buffer, 0, 4);
      numberOfColumns = SwapEndianness(BitConverter.ToInt32(buffer, 0));
      Debug.Log($"Image size: {numberOfRows}x{numberOfColumns}");
    }
  }

  public void ReadNextBatch(int startPoint, int endPoint)
  {
    using (FileStream fileStream = File.OpenRead(fileName))
    {
      // Set pointer to first image
      fileStream.Seek(16 + (startPoint * numberOfColumns), SeekOrigin.Begin);

      int imageCount = endPoint - startPoint;
      images = new float[imageCount][];
      byte[] imageBuffer = new byte[numberOfColumns];

      for (int i = 0; i < imageCount; i++)
      {
        float[] imagePixels = new float[numberOfRows * numberOfColumns];
        // Read Image
        for (int j = 0; j < numberOfRows; j++)
        {
          fileStream.Read(imageBuffer, 0, numberOfColumns);
          int columnIndex = 0;
          foreach (byte b in imageBuffer)
          {
            imagePixels[j * columnIndex] = (float)b / 255.0f;
            columnIndex += 1;
          }
        }
        images[i] = imagePixels;
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