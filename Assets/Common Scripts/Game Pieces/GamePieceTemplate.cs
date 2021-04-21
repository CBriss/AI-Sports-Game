using UnityEngine;

[CreateAssetMenu(fileName = "new game piece", menuName = "Game Pieces/Game Piece")]
public class GamePieceTemplate : ScriptableObject
{
    [SerializeField] public int health;
    [SerializeField] public string tagName;

    [SerializeField] public Sprite image;
    [SerializeField] public Vector2 imageSize;
    [SerializeField] public Vector2 size;
    [SerializeField] public bool useComplexCollider;

    [SerializeField] public bool hasBrain;
    [SerializeField] public int[] brainShape;

    [SerializeField] public GamePieceController componentController;

    [SerializeField] public GameObject prefabObject;

    [SerializeField] public bool hasPhysics;
    [SerializeField] public bool movableByPhysics;
}
