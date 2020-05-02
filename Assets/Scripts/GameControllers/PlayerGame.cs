﻿using UnityEngine;

public class PlayerGame : GameController
{
    public IGame game;

    public void Start()
    {
        game = gameObject.GetComponent<IGame>();
        game.SetGameController(this);
    }

    
    public override void StartGame()
    {
        Vector3 newPlayerNormalizedPosition = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0.5f));
        newPlayerNormalizedPosition.z = 0;
        game.AddPlayer(newPlayerNormalizedPosition);
    }

    public override void Update()
    {
    }
}
