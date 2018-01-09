using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBomb : Box {

    //float health;

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

                audiomanager.PlayBlockBomb();

                //Debug.Log("KABOOM!");
                boxhandler.RemoveBox(this.gameObject, lowerBoxCount);
                Explode();
                return true;
            }
            else
            {
                //GetComponent<Renderer>().material = Resources.Load("Materials/BrickBoxHalfHP", typeof(Material)) as Material;
            }
        }

        return false;
    }

    void Explode()
    {
        float radius = 2.0f;

        List<GameObject> boxes = boxhandler.GetBoxes();
        List<GameObject> boxesToRemove = new List<GameObject>();
        List<bool> booleans = new List<bool>();

        for (int i = 0; i < boxes.Count; i++)
        {
            if(boxes[i] != this.gameObject)
            {
                float diffX = transform.position.x - boxes[i].transform.position.x;
                float diffX2 = Mathf.Pow(diffX, 2);
                float diffY = transform.position.y - boxes[i].transform.position.y;
                float diffY2 = Mathf.Pow(diffY, 2);
                float diffZ = transform.position.z - boxes[i].transform.position.z;
                float diffZ2 = Mathf.Pow(diffZ, 2);

                float sum = diffX2 + diffY2 + diffZ2;

                float sumSQRT = Mathf.Sqrt(sum);

                if (sumSQRT <= radius)
                {
                    boxesToRemove.Add(boxes[i]);
                    booleans.Add(boxes[i].GetComponent<Box>().lowerBoxCount);
                }
            }
        }

        for (int i = 0; i < boxesToRemove.Count; i++)
        {
            boxhandler.RemoveBox(boxesToRemove[i], booleans[i]);
        }
    }
}
