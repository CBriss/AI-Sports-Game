using UnityEngine;

[CreateAssetMenu(fileName = "new component", menuName = "Game Component")]
public class GameComponentTemplate : ScriptableObject
{
    [SerializeField] public Vector2 imageSize;
    [SerializeField] public Vector2 colliderSize;
    [SerializeField] public int health;
    [SerializeField] public Sprite image;
    [SerializeField] public bool isPlayer;
    [SerializeField] public bool hasBrain;
}
