using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerGame : GameController
{
    public IGame game;

    public GamePieceTemplate playerTemplate;
    public GamePieceTemplate obstacleTemplate;

    public GameObject startMenuPrefab;
    public GameObject UIPrefab;

    public static event Action<string[]> OnUpdateUI;

    public void Start()
    {
        game = gameObject.GetComponent<IGame>();
        game.SetPlayerTemplate(playerTemplate);
        game.SetObstacleTemplate(obstacleTemplate);

        game.OnGameStart += StartGameController;
        game.OnGameOver += HandleGameOver;

        GameObject startMenu = Instantiate(startMenuPrefab);
        startMenu.transform.SetParent(gameObject.transform);
        Instantiate(UIPrefab);
    }

    public void LateUpdate()
    {
        if (game.IsActive())
        {
            List<Player> players = game.GetSimulations(true, false)[0].GetActivePlayers();
            if (players.Any())
            {
                string[] UIString = { players[0].Score.ToString() };
                OnUpdateUI(UIString);
            }
        }
    }

    public override void StartGameController()
    {
        game.AddSimulation(1);
        StartCoroutine(game.StartAllSimulations());
    }

    public override void HandleGameOver()
    {
        SceneManager.LoadScene(SceneLoader.Scenes.MainMenuScene.ToString());
    }

    public override GamePieceTemplate GetPlayerTemplate()
    {
        return playerTemplate;
    }
    
    public override GamePieceTemplate GetObstacleTemplate()
    {
        return obstacleTemplate;
    }
}
