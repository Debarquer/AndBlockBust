using UnityEngine;
using System.Collections;

public class NegativeBox : Box
{
    public bool initiated;

    AudioManager audiomanager;

    // Use this for initialization
    void Start()
    {
        audiomanager = GameObject.FindObjectOfType<AudioManager>();
        health = -4;

        initiated = false;

        dmgCDMax = 0.5f;
        dmgCDCurr = 0.0f;

        boxhandler = GameObject.Find("BoxHandler").GetComponent<BoxHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!initiated)
        {
            initiated = true;
            Initiate();
        }

        dmgCDCurr += Time.deltaTime;
    }

    void Initiate()
    {
        health = 1;
        lowerBoxCount = false;
    }

    public override bool Hit(int damage, out bool damageBall)
    {
        damageBall = false;


        //Debug.Log("I got hit, I had " + health + " health");
        if (dmgCDCurr >= dmgCDMax)
        {
            dmgCDCurr = 0;

            health -= damage;
            if (health <= 0)
            {
                audiomanager.PlayBlock();
                //Do stuff
                GameObject.FindGameObjectWithTag("gamehandler").GetComponent<GameHandler>().IncreaseScore(-2);
                boxhandler.RemoveBox(this.gameObject, lowerBoxCount);
                return true;
            }
            else
            {
                GetComponent<Renderer>().material = Resources.Load("Materials/BrickBoxHalfHP", typeof(Material)) as Material;
            }
        }

        return false;
    }
}
