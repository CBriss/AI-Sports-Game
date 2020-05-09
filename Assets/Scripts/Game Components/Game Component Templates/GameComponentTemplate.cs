using UnityEngine;

[CreateAssetMenu(fileName = "new game component", menuName = "Game Components/Game Component")]
public class GameComponentTemplate : ScriptableObject
{
    [SerializeField] public int health;
    [SerializeField] public string tagName;

    [SerializeField] public Sprite image;
    [SerializeField] public Vector2 imageSize;
    [SerializeField] public Vector2 colliderSize;
    
    [SerializeField] public bool hasBrain;
    [SerializeField] public int[] brainShape;

    [SerializeField] public ComponentController componentController;

}
