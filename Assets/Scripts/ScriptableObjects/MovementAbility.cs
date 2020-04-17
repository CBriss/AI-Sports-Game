using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementAbility : ScriptableObject
{
    public abstract void Move(GameObject gameObject);
}
