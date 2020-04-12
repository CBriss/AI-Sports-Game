using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public float backgroundSpeed;
    public Renderer backgroundRenderer;

    void Update()
    {
        backgroundRenderer.material.mainTextureOffset += new Vector2(0f, backgroundSpeed * Time.deltaTime);  
    }
}
