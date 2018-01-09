using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBox : Box {

    public bool initiated;

    //float health;
    AudioManager audiomanager;

    // Use this for initialization
    void Start()
    {
        audiomanager = GameObject.FindObjectOfType<AudioManager>();
        health = -3;

        initiated = false;

        dmgCDMax = 0.2f;
        dmgCDCurr = 0.0f;

        boxhandler = GameObject.Find("BoxHandler").GetComponent<BoxHandler>();
    }

    // Update is called once per frame
    void Update () {
        if (!initiated)
        {
            initiated = true;
            Initiate();
        }

        dmgCDCurr += Time.deltaTime;
    }

    void Initiate()
    {
        //Debug.Log("Initiating");
        health = 2;
        lowerBoxCount = true;
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
                boxhandler.RemoveBox(this.gameObject, lowerBoxCount);
                return true;
            }
            else
            {
                audiomanager.PlayBlockBrick();
                GetComponent<Renderer>().material = Resources.Load("Materials/BrickBoxHalfHP", typeof(Material)) as Material;
            }
        }

        return false;
    }

}
