using UnityEngine;

public abstract class ComponentController : ScriptableObject
{
    public abstract void UpdateComponent(GameComponent gameComponent);
}
