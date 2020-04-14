using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Graph : MonoBehaviour
{
  [SerializeField] private Sprite dotSprite;
  [SerializeField] private RectTransform graphContainer;

  float graphHeight;
  float yMax = 100f;
  float xMax = 400f;

  void Start()
  {
    graphHeight = graphContainer.sizeDelta.y;
  }

  public void ShowGraph(List<float> valueList)
  {
    ClearGraph();
    List<float> normalizedValueList = NormalizeHeights(valueList);
    float xDistance = NormalizedXDistance(valueList.Count);
    for (int i = 0; i < normalizedValueList.Count; i++)
    {
      float xPostion = i * xDistance;
      float yPosition = normalizedValueList[i];
      CreateCircle(new Vector2(xPostion, yPosition));
    }
  }

  public void ClearGraph(){
    foreach(GameObject dot in GameObject.FindGameObjectsWithTag("GraphDot")){
      Destroy(dot);
    }
  }

  private void CreateCircle(Vector2 anchoredPosition)
  {
    GameObject newDot = new GameObject("graph_dot", typeof(Image));
    newDot.gameObject.tag="GraphDot"; 
    newDot.transform.SetParent(graphContainer, false);
    newDot.GetComponent<Image>().sprite = dotSprite;
    RectTransform rectTransform = newDot.GetComponent<RectTransform>();
    rectTransform.anchoredPosition = anchoredPosition;
    rectTransform.sizeDelta = new Vector2(2, 2);
    rectTransform.anchorMin = new Vector2(0, 0);
    rectTransform.anchorMax = new Vector2(0, 0);
  }

  private List<float> NormalizeHeights(List<float> valueList)
  {
    float maxValue = 0.0f;
    foreach (float value in valueList)
    {
      if (value > maxValue)
        maxValue = value;
    }

    List<float> normalizedValueList = new List<float>();
    foreach (float value in valueList)
    {
      normalizedValueList.Add((value / maxValue) * yMax);
    }

    return normalizedValueList;
  }

  private float NormalizedXDistance(int valueSize)
  {
    return (xMax / valueSize);
  }

}
