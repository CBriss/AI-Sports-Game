using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    public void Awake()
    {
        current = this;
    }

    public event Action onCollisionDetected;
    public void CollisionDetected()
    {
        onCollisionDetected?.Invoke();
    }
}
