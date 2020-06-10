using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA : MonoBehaviour
{
  public List<GeneNode> GeneNodes;
  public List<GeneConnection> GeneConnections;

  public struct GeneNode
  {
    public int id;
    public int layer;
    public List<GeneNode> myConnectedNodes;

    public GeneNode(int id, int layer) : this()
    {
      this.id = id;
      this.layer = layer;
    }
  }

  public struct GeneConnection
  {
    public int Innovation;
    public GeneNode Input;
    public GeneNode Output;
    public float Weight;
    public bool Enabled;

    public GeneConnection(int Innovation, GeneNode Input, GeneNode Output, float Weight, bool Enabled) : this()
    {
      this.Innovation = Innovation;
      this.Input = Input;
      this.Output = Output;
      this.Weight = Weight;
      this.Enabled = Enabled;
    }
  }

  void Start()
  {

  }

  void Update()
  {

  }

  // a single new connection gene with a random weight is added connecting
  // two previously unconnected nodes
  // --NOTE-- account for situations where all connections are already made
  void AddConnection(int innovationNumber)
  {
    GeneNode newInputNode = GeneNodes[Random.Range(0, GeneNodes.Count)];
    GeneNode newOutputNode = GeneNodes[Random.Range(0, GeneNodes.Count)];
    while (newInputNode.myConnectedNodes.Contains(newOutputNode))
    {
      newInputNode = GeneNodes[Random.Range(0, GeneNodes.Count)];
      newOutputNode = GeneNodes[Random.Range(0, GeneNodes.Count)];
    }

    GeneConnections.Add(new GeneConnection(
        innovationNumber, newInputNode, newOutputNode, Random.Range(0f, 1f), true
    ));
    newInputNode.myConnectedNodes.Add(newOutputNode);
    newOutputNode.myConnectedNodes.Add(newInputNode);
  }

  // In the add node mutation, an existing connection is split and the new node placed where the old connection used to be.The old connection
  // is disabled and two new connections are added to the genome.The new connection
  // leading into the new node receives a weight of 1, and the new connection leading out
  // receives the same weight as the old connection.
  /*
  void AddNode()
  {
      // Randomly pick a connection from A to B
      GeneConnection randomConnection = GeneConnections[Random.Range(0, GeneConnections.Count)];

      // create new node
      GeneNode newNode = new GeneNode(GeneNodes.Count, randomConnection.Output);

      // make connection from A to new node with weight 1
      GeneConnection newConnectionA = new GeneConnection(
          GeneConnections.Count, randomConnection.Input, newNode, 1, true
      );

      // make connection from new node to B with same weight as old connection
      GeneConnection newConnectionB = new GeneConnection(
          GeneConnections.Count, newNode, randomConnection.Input, randomConnection.Weight, true
      );

      // update A's connectedNodes
      randomConnection.Input.connectedNodes.Remove(randomConnection.Output);
      randomConnection.Input.connectedNodes.Add(newNode);

      // Set new node's connectedNodes to B
      newNode.connectedNodes.Add(randomConnection.Output);

      // Disable oldConnection
      randomConnection.Enabled = false;
  }
  */

  /*
   * public int[] Think()
  {
      // loop
      return true;
  }
  */
}
