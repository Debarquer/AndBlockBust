using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class Powers : MonoBehaviour {

    public BoxHandler boxhandler;
    public GameHandler gamehandler;
    public MenuManager menumanager;

    public GameObject ActivatingTouchButton;
    public UnityEngine.UI.Text ActivatingTouchUses1;
    public UnityEngine.UI.Text ActivatingTouchUses2;
    public GameObject ActivatingTouchCancel;

    public UnityEngine.UI.Text HealingUses;
    public UnityEngine.UI.Text SpawnBallUses;

    bool ActivatingTouchActive;

    Dictionary<string, int> powers;

	// Use this for initialization
	void Start () {
        ActivatingTouchActive = false;

        powers = new Dictionary<string, int>();

        powers.Add("Healing", 5);
        powers.Add("ActivatingTouch", 5);
        powers.Add("SpawnBall", 5);

        ActivatingTouchUses1.text = 5.ToString();
        ActivatingTouchUses2.text = 5.ToString();
        HealingUses.text = 5.ToString();
        SpawnBallUses.text = 5.ToString();
    }
	
	// Update is called once per frame
	void Update () {
        if (ActivatingTouchActive)
        {
            if (Input.GetMouseButtonUp(0))
            {
                //Debug.Log("Act. Touch at " + Input.mousePosition);
                ActivatingTouch(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
        }
	}

    public void ResetPowerUses()
    {
        powers["Healing"] = 5;
        powers["ActivatingTouch"] = 5;
        powers["SpawnBall"] = 5;
    }

    public void WatchAdForDoublePowerUses()
    {
        var options = new ShowOptions { resultCallback = HandleShowResult };

        if (Advertisement.IsReady("rewardedVideo"))
        {
            Advertisement.Show("rewardedVideo", options);
            return;
        }
        else
        {
            Debug.Log("Ad not ready");
        }

        menumanager.StartGame();
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                DoublePowerUses();
                menumanager.StartGame();
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                menumanager.StartGame();
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                menumanager.StartGame();
                break;
        }
    }

    private void DoublePowerUses()
    {
        powers["Healing"] *= 2;
        powers["ActivatingTouch"] *= 2;
        powers["SpawnBall"] *= 2;

        ActivatingTouchUses1.text = powers["ActivatingTouch"].ToString();
        ActivatingTouchUses2.text = powers["ActivatingTouch"].ToString();
        HealingUses.text = powers["Healing"].ToString();
        SpawnBallUses.text = powers["SpawnBall"].ToString();
    }

    public void AddPowerUse()
    {
        powers["Healing"] += 1;
        powers["ActivatingTouch"] += 1;
        powers["SpawnBall"] += 1;

        ActivatingTouchUses1.text = powers["ActivatingTouch"].ToString();
        ActivatingTouchUses2.text = powers["ActivatingTouch"].ToString();
        HealingUses.text = powers["Healing"].ToString();
        SpawnBallUses.text = powers["SpawnBall"].ToString();
    }

    public void ToggleActivatingTouch(bool active)
    {
        if (active)
        {
            Debug.Log("Activating touch is active");
            ActivatingTouchActive = true;
            ActivatingTouchButton.SetActive(false);
            //ActivatingTouchUses1.enabled = false;
            //ActivatingTouchUses2.enabled = true;
            ActivatingTouchCancel.SetActive(true);

            gamehandler.mousestate = GameHandler.MouseState.Unavailable;
        }
        else if(!active)
        {
            Debug.Log("Activating touch is inactive");
            ActivatingTouchActive = false;
            ActivatingTouchButton.SetActive(true);
            ActivatingTouchCancel.SetActive(false);

            gamehandler.mousestate = GameHandler.MouseState.Available;
        }
        else
        {
            Debug.Log("Activating touch neither true nor false");
        }
    }

    void ActivatingTouch(Vector3 clickPos)
    {
        if (powers["ActivatingTouch"] > 0)
        {
            List<GameObject> boxes = boxhandler.GetBoxes();
            for (int i = 0; i < boxes.Count; i++)
            {
                float boxLength = boxes[i].GetComponent<BoxCollider>().bounds.extents.x;
                float boxHeight = boxes[i].GetComponent<BoxCollider>().bounds.extents.y;

                //Debug.Log("Length Height " + boxLength + " " + boxHeight);

                float boxMaxX = boxes[i].transform.position.x + boxLength;
                float boxMinX = boxes[i].transform.position.x - boxLength;
                float boxMaxY = boxes[i].transform.position.y + boxHeight;
                float boxMinY = boxes[i].transform.position.y - boxHeight;
                //Debug.Log("Box max min max min " + boxMaxX + " " + boxMinX + " " + boxMaxY + " " + boxMinY);
                if (clickPos.x < boxMaxX && clickPos.x > boxMinX && clickPos.y < boxMaxY && clickPos.y > boxMinY)
                {
                    bool damageBall;
                    boxes[i].GetComponent<Box>().Hit(1, out damageBall);
                    powers["ActivatingTouch"]--;
                    ActivatingTouchUses1.text = powers["ActivatingTouch"].ToString();
                    ActivatingTouchUses2.text = powers["ActivatingTouch"].ToString();
                }
            }
        }
        else
        { 
            Debug.Log("No more uses of activating touch");
        }
    }

    public void IncreaseHP()
    {
        if(powers["Healing"] > 0)
        {
            gamehandler.GainLife();
            powers["Healing"]--;
            HealingUses.text = powers["Healing"].ToString();
        }
        else
        {
            Debug.Log("No more uses of healing");
        }
    }

    public void Spawnball()
    {
        if(powers["SpawnBall"] > 0)
        {
            gamehandler.SpawnBall();

            powers["SpawnBall"]--;
            SpawnBallUses.text = powers["SpawnBall"].ToString();
        }
    }
}
