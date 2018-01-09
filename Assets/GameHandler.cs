using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Advertisements;

public class GameHandler : MonoBehaviour {

    public string username;
    public string id;

    public BoxHandler boxhandler;
    public Ball ball;
    public Player player;
    public MenuManager menumanager;

    public GameObject GameOverScreen;

    public enum GameModes { Invalid, Story, Endless};
    public GameModes gamemode;

    public enum GameState { MainMenu, Playing, Paused, MouseOccupied, GameOver };
    public enum MouseState { Invalid, Available, Unavailable};
    public GameState gamestate;
    public MouseState mousestate;

    public float score;// = 0;
    public float level;
    public int playerLevel = 0;
    public int requiredXPremaining = 0;

    public UnityEngine.UI.Text ScoreText;
    public UnityEngine.UI.Text LivesText;
    public UnityEngine.UI.Text LevelText;
    public UnityEngine.UI.Text GameOverText;
    public UnityEngine.UI.Text GameOverText2;
    public GameObject PauseTextBackground;
    public UnityEngine.UI.Text RegisterUsername;
    //public UnityEngine.UI.Text ScoreboardText;

    public GameObject leaderboardContent1;
    public GameObject leaderboardContent2;
    public GameObject leaderboardEntry;

    public GameObject Unregistered;
    public GameObject Registered;

    public UnityEngine.UI.Text regUsername;
    public UnityEngine.UI.Text regLevel;
    public UnityEngine.UI.Text regExperience;
    public UnityEngine.UI.Text regExperienceRequiredA;
    public UnityEngine.UI.Text regExperienceRequiredB;
    public UnityEngine.UI.Text regRankFriends;
    public UnityEngine.UI.Text regRankGlobal;

    public UnityEngine.UI.Text GameoverLevelText;
    public UnityEngine.UI.Text GameoverTotalText;

    public GameObject MMXPBarInnerA;
    public GameObject MMXPBarInnerB;
    public float MMTargetScale;
    public int amountOfTimesLevelUp = 0;

    public GameObject PlayGameButton;
    public GameObject ResumeGameButton;

    public float lives;
    public float _nrOfBalls;

    bool initiated;

    public GameObject _ballPrefab;

    public GameObject LoadingScreen;

    List<GameObject> _balls;

    public AudioManager audiomanager;

    // Use this for initialization
    void Start() {
        Debug.Log("Start");

        username = "Not logged in";

        initiated = false;

        //RegisterAndAuthenticateDevice();

        gamemode = GameModes.Endless;
        gamestate = GameState.MainMenu;
        mousestate = MouseState.Unavailable;

        lives = 1;
        level = 1;
        score = 0;
        _nrOfBalls = 1;

        GameOverScreen.SetActive(false);
        GameOverText.enabled = false;
        GameOverText2.enabled = false;
        PauseTextBackground.SetActive(false);

        leaderboardEntry = (GameObject)Resources.Load("Entry");

        MMTargetScale = MMXPBarInnerB.transform.localScale.x;

        _balls = new List<GameObject>();

        UpdateUI();
    }

    // Update is called once per frame
    void Update() {

        if (mousestate == GameHandler.MouseState.Available && (gamestate == GameHandler.GameState.Playing || gamestate == GameHandler.GameState.Paused))
        {
            if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyUp(KeyCode.Space))
            {
                //Debug.Log(Input.mousePosition.x);
                if (Input.mousePosition.x < (Screen.width - 100))
                {
                    if (gamestate == GameHandler.GameState.Playing)
                    {
                        gamestate = GameHandler.GameState.Paused;
                        PauseTextBackground.SetActive(true);
                    }
                    else if (gamestate == GameHandler.GameState.Paused)
                    {
                        gamestate = GameHandler.GameState.Playing;
                        PauseTextBackground.SetActive(false);
                    }
                    else if (gamestate == GameHandler.GameState.GameOver)
                    {
                        ResetGame(false);
                    }
                }
            }
        }

        if (!initiated)
        {
            initiated = true;

            UnityEngine.UI.Text[] tmp = Registered.GetComponentsInChildren<UnityEngine.UI.Text>();
            foreach (UnityEngine.UI.Text txt in tmp)
            {
                //Debug.Log("Setting value of " + txt.name);
                switch (txt.name)
                {
                    case "Username":
                        regUsername = txt;
                        break;
                    case "Level":
                        regLevel = txt;
                        break;
                    case "Experience":
                        regExperience = txt;
                        break;
                    case "RankFriends":
                        regRankFriends = txt;
                        break;
                    case "RankGlobal":
                        regRankGlobal = txt;
                        break;
                }
            }

            RegisterAndAuthenticateDevice();

            UpdateLeaderboards();
        }

        //if(_nrOfBalls <= 0)
        //{
        //    ResetPlayer();
        //}

        AnimateXPbar();
    }

    public void AdvanceLevel()
    {
        level++;
        //ball.Reset(true);

        for(int i = 0; i < _balls.Count; i++)
        {
            //GameObject tmp = _balls[i];
            //_balls.Remove(tmp);

            if (_balls[i] != null)
                Destroy(_balls[i]);
            else
                Debug.Log("Unable to destroy ball because it is null for some reason");
        }

        _balls.Clear();

        SpawnBall();
        _nrOfBalls = 1;

        boxhandler.Reset();
        player.Reset();
        UpdateUI();
        gamestate = GameState.Paused;
        PauseTextBackground.SetActive(true);

        audiomanager.PlayAdvanceLevel();
    }

    public void ResetGame(bool ShowResumeButton)
    {
        for (int i = 0; i < _balls.Count; i++)
        {
            //GameObject tmp = _balls[i];
            //_balls.Remove(tmp);

            if (_balls[i] != null)
                Destroy(_balls[i]);
            else
                Debug.Log("Unable to destroy ball because it is null for some reason");
        }

        _balls.Clear();

        ResetPlayer();
        SpawnBall();
        _nrOfBalls = 1;
        ResetScore();
        boxhandler.Reset();

        //ball.Show();

        if(ShowResumeButton)
        {
            PlayGameButton.SetActive(false);
            ResumeGameButton.SetActive(true);
        }
        else
        {
            PlayGameButton.SetActive(true);
            ResumeGameButton.SetActive(false);
        }
    }

    public void ResetPlayer()
    {
        player.Reset();
        if(gamestate != GameState.MainMenu)
            gamestate = GameState.Paused;
        PauseTextBackground.SetActive(true);
    }

    void ResetScore()
    {
        lives = 1;
        score = 0;
        level = 1;
        UpdateUI();
    }

    public void DestroyBall(GameObject ball)
    {

        _balls.Remove(ball);
        Destroy(ball);

        _nrOfBalls--;
        if(_nrOfBalls <= 0)
        {
            LoseLife();

            audiomanager.PlayLoseLife();

            if(gamestate != GameState.GameOver)
            {
                ResetPlayer();
                SpawnBall();
            }
            else
            {

            }
        }
    }

    void LoseLife()
    {
        lives--;
        if(lives < 0)
        {
            lives = 0;

            //ball.Hide();

            gamestate = GameState.GameOver;
            menumanager.GameOver();
            //ResetGame(false);
        }

        UpdateUI();
    }

    public void GainLife()
    {
        audiomanager.PlayIncreaseHealth();
        lives++;
        UpdateUI();
    }

    public void IncreaseScore(float multiplier = 1.0f)
    {
        int increaseScoreBy = (int)(10 * multiplier);

        score += increaseScoreBy;

        UpdateUI();
    }

    void UpdateUI()
    {
        ScoreText.text = "Score: " + score;
        LivesText.text = "Lives: " + lives;
        LevelText.text = "Level: " + level;
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void UpdateLeaderboards()
    {
        new GameSparks.Api.Requests.LeaderboardDataRequest().SetLeaderboardShortCode("TST").SetEntryCount(10).Send((response) => {
        if (!response.HasErrors)
        {
            bool leaderboardsActive = leaderboardContent1.activeInHierarchy | leaderboardContent2.activeInHierarchy;

            int id = 0;
            int leaderboardEntryY;
            if (leaderboardContent1.activeInHierarchy)
            {
                leaderboardEntryY = 85;
            }
            else
            {
                leaderboardEntryY = 135;
            }

            GameObject[] tmp = GameObject.FindGameObjectsWithTag("LeaderBoardEntry");
            List<GameObject> LeaderboardEntries = new List<GameObject>();
            if (leaderboardsActive)
            {
                for (int i = 0; i <= 10; i++)
                {
                    foreach (GameObject entry in tmp)
                    {
                        if (entry.name == i.ToString())
                        {
                            //Debug.Log("Added entry " + i);
                            LeaderboardEntries.Add(entry);
                            entry.SetActive(false);
                            break;
                        }
                    }
                }
            }
            

            string ScoreboardTextString = "";
            //Debug.Log("Found Leaderboard Data...");
            foreach (GameSparks.Api.Responses.LeaderboardDataResponse._LeaderboardData entry in response.Data)
            {
                int rank = (int)entry.Rank;
                string playerName = entry.UserName;
                string score = entry.JSONData["SCORER"].ToString();
                //Debug.Log("Rank:" + rank + " Name:" + playerName + " Score:" + score + "\n");
                ScoreboardTextString += "Rank:" + rank + " Name:" + playerName + " Score:" + score + "\n";

                if (playerName == username)
                    regRankGlobal.text = "Current ranking: " + rank.ToString();

                //LeaderboardEntries[id].transform.localPosition = new Vector3(0, leaderboardEntryY, 0);
                if (leaderboardsActive)
                {
                    UnityEngine.UI.Text[] texts = LeaderboardEntries[id].GetComponentsInChildren<UnityEngine.UI.Text>();
                    foreach (UnityEngine.UI.Text text in texts)
                    {
                        switch (text.gameObject.name)
                        {
                            case "RankText":
                                text.text = rank.ToString();
                                break;
                            case "UsernameText":
                                text.text = playerName;
                                break;
                            case "ScoreText":
                                text.text = score;
                                break;
                        }
                    }
                    LeaderboardEntries[id].SetActive(true);
                    leaderboardEntryY -= 30;
                }
                
                //Debug.Log("id"+id);
                id++;
            }

            Debug.Log(ScoreboardTextString);
            //ScoreboardText.text = ScoreboardTextString;
            }
            else
            {
                Debug.Log("Error Retrieving Leaderboard Data... \n" + response.Errors);
            }
        });
    }

    public void PostToLeaderboard(float score)
    {
        //Let's be cool
        if (score < 0)
            return;

        new GameSparks.Api.Requests.LogEventRequest()
        .SetEventKey("TST")
        .SetEventAttribute("SCORER", (int)score)
        .Send((response) => {
            if (!response.HasErrors)
            {
            }
            else
            {
                Debug.Log("Error Posting to Leaderboard... \n" + response.Errors);
            }
        });
    }

    public void GetPlayerXP(string screenname)
    {
        new GameSparks.Api.Requests.LeaderboardDataRequest().SetLeaderboardShortCode("xpLeaderboard").SetEntryCount(1000).Send((response) =>
        {
            if (!response.HasErrors)
            {
                foreach (GameSparks.Api.Responses.LeaderboardDataResponse._LeaderboardData entry in response.Data)
                {
                    if (entry.UserName == username)
                    {
                        //this is the person!!

                        //int GameoverLevelInt = CalculateLevel(int.Parse(entry.JSONData["expAttribute"].ToString()));

                        int xp = -1;
                        if(int.TryParse(entry.JSONData["expAttribute"].ToString(), out xp))
                        {
                            int GameoverLevelInt = 0;
                            int GameoverExpReq = 0;
                            CalculateLevel(xp, out GameoverLevelInt, out GameoverExpReq);
                            regLevel.text = "Level: " + GameoverLevelInt.ToString();
                            regExperience.text = GameoverTotalText.text = "Total XP: " + entry.JSONData["expAttribute"].ToString();
                            regExperienceRequiredA.text = "XP to next level: " + GameoverExpReq.ToString();
                            if(amountOfTimesLevelUp > 0)
                                regExperienceRequiredB.text = "XP to next level: 0";
                            else
                                regExperienceRequiredB.text = "XP to next level: " + GameoverExpReq.ToString();
                        }
                        else
                        {
                            Debug.Log(entry.JSONData["expAttribute"].ToString() + " failed to parse as an int");
                        }
                    }
                }

                LoadingScreen.SetActive(false);
            }
            else
            {

            }
        });
    }

    public void PostXP(float xp)
    {
        //Let's be cool
        if (xp < 0)
            return;

        new GameSparks.Api.Requests.LogEventRequest()
        .SetEventKey("expEvent")
        .SetEventAttribute("expAttribute", (int)xp)
        .Send((response) => {
            if (!response.HasErrors)
            {

            }
            else
            {
                Debug.Log("Error Posting to Leaderboard... \n" + response.Errors);
            }
        });
    }

    public void RegisterAndAuthenticateDevice()
    {
        new GameSparks.Api.Requests.DeviceAuthenticationRequest() 
            .SetDisplayName("NullUser")
            .Send((response) => {
                if (!response.HasErrors)
                {
                    Debug.Log("Signed in as " + response.DisplayName);
                    username = response.DisplayName;
                    id = response.UserId;

                    if(response.DisplayName == "NullUser")
                    {
                        //DeletePlayer(response.UserId);
                        Debug.Log("Unregistered device");
                        PlayerRegisteredOrNot(false);

                        //SetUsername("SomethingElseAgain");
                    }
                    else
                    {
                        PlayerRegisteredOrNot(true);
                        regUsername.text = "Greetings " + username;
                        GetPlayerXP(username);
                    }

                    LoadingScreen.SetActive(false);
                }
                else
                {
                    Debug.Log("Error Posting to Leaderboard... \n" + response.Errors);
                }
            });
    }

    public void SetUsername(string newDisplayname)
    {
        new GameSparks.Api.Requests.ChangeUserDetailsRequest().SetDisplayName(RegisterUsername.text)
            .Send((response) =>
            {
                if (!response.HasErrors)
                {
                    RegisterAndAuthenticateDevice();
                }
            });
    }

    public void DeletePlayer(string player)
    {
        new GameSparks.Api.Requests.LogEventRequest()
            .SetEventKey("deletePlayer")
            .SetEventAttribute("player", player)
            .Send((response) =>
            {
                Debug.Log("Player not registered, deleting superfluous entry");
            });
    }

    public void PlayerRegisteredOrNot(bool registered)
    {
        //Debug.Log(registered);
        Unregistered.SetActive(!registered);
        Registered.SetActive(registered);
    }

    public void CalculateLevel(int b, out int level, out int expReq)
    {
        //return xp / 100;


        //calculate CURRENT level
        int a = (int)(Mathf.Sqrt(625+(100*b))-25) / 50;

        int xpReq = 25 * (int)(Mathf.Pow(a + 1, 2) + a + 1);
        int xpReqPrevLevel = 25 * (int)(Mathf.Pow(a, 2) + a);

        //Debug.Log("level " + a + " xp required for next level " + xpReq);

        //current level
        level = a + 1;
        expReq = xpReq - b;
        requiredXPremaining = expReq;

        //calculate previous level
        float xpGained = score;
        int c = (int)(Mathf.Sqrt(625 + (100 * (b-xpGained))) - 25) / 50;
        int prevLevel = playerLevel = c + 1;

        GameoverLevelText.text = "Level: " + prevLevel.ToString();

        amountOfTimesLevelUp = level - prevLevel;

        Debug.Log(" XpGained(score), prevlevel, newlevel, difference " + xpGained + ", " + prevLevel + ", " + level + ", " + (level - prevLevel));

        float xpLevelDifference = xpReq - xpReqPrevLevel;
        float xpIHaveAccumulatedSinceLastLevel = b - xpReqPrevLevel;
        float percentage = xpIHaveAccumulatedSinceLastLevel/ xpLevelDifference;
        //Debug.Log("xpReq " + xpReq + " xpReqPrevLevel " + xpReqPrevLevel + " Difference " + xpLevelDifference + " xpIHaveAccumulated " + xpIHaveAccumulatedSinceLastLevel + " percentage " + percentage);

        MMXPBarInnerA.transform.localScale = new Vector3(percentage, 1.0f, 1.0f);
        MMXPBarInnerA.transform.localPosition = new Vector3(-((375.0f * (1.0f-percentage))/2), MMXPBarInnerA.transform.localPosition.y, MMXPBarInnerA.transform.localPosition.z);

        MMTargetScale = percentage;

        //MMXPBarInnerB.transform.localScale = new Vector3(percentage, 1.0f, 1.0f);
        //MMXPBarInnerB.transform.localPosition = new Vector3(-((375.0f * (1.0f - percentage)) / 2), MMXPBarInnerB.transform.localPosition.y, MMXPBarInnerB.transform.localPosition.z);
    }

    public void AnimateXPbar()
    {
        float currentScale = MMXPBarInnerB.transform.localScale.x;

        if (amountOfTimesLevelUp > 0)
        {
            audiomanager.StartLevelUpLoop();

            regExperienceRequiredB.text = "XP to next level: 0";
            if (currentScale >= 1.0f)
            {
                audiomanager.PlayLevelUp();

                amountOfTimesLevelUp--;
                playerLevel++;
                GameoverLevelText.text = "Level: " + playerLevel.ToString();

                float resetbar = 0.0f;

                MMXPBarInnerB.transform.localScale = new Vector3(resetbar, 1.0f, 1.0f);
                MMXPBarInnerB.transform.localPosition = new Vector3(-((375.0f * (1.0f - resetbar)) / 2), MMXPBarInnerB.transform.localPosition.y, MMXPBarInnerB.transform.localPosition.z);

                if(amountOfTimesLevelUp > 0) 
                    return;
                else
                {
                    regExperienceRequiredB.text = "XP to next level: " + requiredXPremaining.ToString();
                }

                return;
            }
        }
        else
        {
            if (MMTargetScale - currentScale < 0.01f)
            {
                audiomanager.StopLevelUpLoop();
                return;
            }
        }

        //Debug.Log("Animating xp bar from " + currentScale + " to " + MMTargetScale + " with a change of " + (currentScale + (Time.deltaTime/10)));

        audiomanager.StartLevelUpLoop();

        float percentage = currentScale + (Time.deltaTime/5);

        MMXPBarInnerB.transform.localScale = new Vector3(percentage, 1.0f, 1.0f);
        MMXPBarInnerB.transform.localPosition = new Vector3(-((375.0f * (1.0f - percentage)) / 2), MMXPBarInnerB.transform.localPosition.y, MMXPBarInnerB.transform.localPosition.z);
    }

    public void SpawnBall()
    {
        SpawnBall(new Vector3(8.5f, -3.0f, 0.0f));
    }

    public void SpawnBall(Vector3 location)
    {
        Debug.Log("Spawning ball");

        _nrOfBalls++;

        _balls.Add(Instantiate(_ballPrefab, location, Quaternion.identity));
    }

    public void LoadBallPrefab(string name)
    {
        GameObject tmp = (GameObject)Resources.Load("BallPrefabs/" + name);
        if(tmp == null)
        {
            Debug.Log("Failed to load ball prefab " + name);
            return;
        }

        _ballPrefab = tmp;
    }
}