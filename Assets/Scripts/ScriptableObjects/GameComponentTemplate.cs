using System;
using UnityEngine;

[CreateAssetMenu(fileName = "new game component", menuName = "Game Components/Game Component")]
public class GameComponentTemplate : ScriptableObject
{
    [SerializeField] public Vector2 imageSize;
    [SerializeField] public Vector2 colliderSize;
    [SerializeField] public int health;
    [SerializeField] public Sprite image;
    [SerializeField] public bool isPlayer;
    [SerializeField] public bool hasBrain;
    [SerializeField] public int[] brainShape;
    [SerializeField] public MovementAbility movementAbility;
    [SerializeField] public string tagName;
}
