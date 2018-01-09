using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

    public Canvas MainMenuCanvas;
    public Canvas EndlessModeCanvas;

    public GameHandler gamehandler;

    public GameObject gameoverscreen;

    bool initiated;

    public GameObject _boxes;
    public GameObject _sideBox;

	// Use this for initialization
	void Start () {
        MainMenuCanvas.gameObject.SetActive(true);
        EndlessModeCanvas.gameObject.SetActive(false);

        initiated = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!initiated)
        {
            initiated = true;
        }
	}

    public void StartGame()
    {
        //gamehandler.RegisterAndAuthenticateDevice();

        MainMenuCanvas.gameObject.SetActive(false);
        EndlessModeCanvas.gameObject.SetActive(true);

        gamehandler.gamestate = GameHandler.GameState.Paused;
        gamehandler.mousestate = GameHandler.MouseState.Available;
        gamehandler.PauseTextBackground.SetActive(true);

        gamehandler.PlayGameButton.SetActive(false);
        gamehandler.ResumeGameButton.SetActive(true);
    }

    public void OpenMenu()
    {
        gamehandler.GetPlayerXP(gamehandler.username);

        MainMenuCanvas.gameObject.SetActive(true);
        EndlessModeCanvas.gameObject.SetActive(false);

        gamehandler.gamestate = GameHandler.GameState.MainMenu;
        gamehandler.mousestate = GameHandler.MouseState.Available;
        gamehandler.PauseTextBackground.SetActive(true);
    }

    public void GameOver()
    {
        gamehandler.mousestate = GameHandler.MouseState.Unavailable;
        gamehandler.gamestate = GameHandler.GameState.GameOver;

        gamehandler.PlayGameButton.SetActive(true);
        gamehandler.ResumeGameButton.SetActive(false);

        MainMenuCanvas.gameObject.SetActive(true);
        EndlessModeCanvas.gameObject.SetActive(false);

        gameoverscreen.SetActive(true);
        gamehandler.PostToLeaderboard(gamehandler.score);
        gamehandler.PostXP(gamehandler.score);
        gamehandler.GetPlayerXP(gamehandler.username);
        gamehandler.UpdateLeaderboards();

        UnityEngine.UI.Text[] tmp = gameoverscreen.GetComponentsInChildren<UnityEngine.UI.Text>();
        foreach(UnityEngine.UI.Text txt in tmp)
        {
            if(txt.name == "GameoverGained")
            {
                if (gamehandler.score >= 0)
                    txt.text = "XP gained: +" + gamehandler.score;
                else
                    txt.text = "XP gained: 0";
            }
        }

        Debug.Log("Finished GameOver function");
    }
}
