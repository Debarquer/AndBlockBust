using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxHandler : MonoBehaviour {

    List<GameObject> boxTemplates;
    List<GameObject> boxes;
    public float nrOfBoxes;
    public GameHandler gamehandler;
    public RandomNumberGenerator RanNumGen;

    public Transform parent;

    bool initiated;

    enum BoxEnum { Box, BoxBrick, BoxBomb, BoxHeal, AddPowerUsesBox, BadBox, DeadlyBox, BoxBall};

	// Use this for initialization
	void Start () {
        boxTemplates = new List<GameObject>();
        boxTemplates.Add((GameObject)Resources.Load("Box"));
        boxTemplates.Add((GameObject)Resources.Load("BoxBrick"));
        boxTemplates.Add((GameObject)Resources.Load("BoxBomb"));
        boxTemplates.Add((GameObject)Resources.Load("BoxHeal"));
        boxTemplates.Add((GameObject)Resources.Load("AddPowerUsesBox"));
        boxTemplates.Add((GameObject)Resources.Load("BadBox"));
        boxTemplates.Add((GameObject)Resources.Load("DeadlyBox"));
        boxTemplates.Add((GameObject)Resources.Load("BoxBall"));

        boxes = new List<GameObject>();

        nrOfBoxes = 0;
        initiated = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (!initiated)
        {
            initiated = true;
            InstantiateAllBoxes();
        }	
	}

    void InstantiateBox(Vector3 position)
    {
        int id = RanNumGen.GenerateRandomNumber(100);
        //id = Mathf.Clamp(id, 0, 1);

        if(id <= 70)
        {
            GameObject tmp = Instantiate(boxTemplates[(int)BoxEnum.Box], position, Quaternion.identity);
            tmp.transform.Rotate(new Vector3(-90, 0));
            tmp.transform.SetParent(parent);
            boxes.Add(tmp);
            nrOfBoxes++;
        }
        else if (id > 70 && id <= 88)
        {
            GameObject tmp = Instantiate(boxTemplates[(int)BoxEnum.BoxBall], position, Quaternion.identity);
            tmp.transform.Rotate(new Vector3(-90, 0));
            tmp.transform.SetParent(parent);
            boxes.Add(tmp);
            nrOfBoxes++;
        }
        else if (id > 88 && id <= 90)
        {
            GameObject tmp = Instantiate(boxTemplates[(int)BoxEnum.AddPowerUsesBox], position, Quaternion.identity);
            tmp.transform.Rotate(new Vector3(-90, 0));
            tmp.transform.SetParent(parent);
            boxes.Add(tmp);
            nrOfBoxes++;
        }
        else if (id > 90 && id <= 92)
        {
            GameObject tmp = Instantiate(boxTemplates[(int)BoxEnum.BadBox], position, Quaternion.identity);
            tmp.transform.Rotate(new Vector3(-90, 0));
            tmp.transform.SetParent(parent);
            boxes.Add(tmp);
            //nrOfBoxes++;
        }
        else if (id > 92 && id <= 94)
        {
            GameObject tmp = Instantiate(boxTemplates[(int)BoxEnum.DeadlyBox], position, Quaternion.identity);
            tmp.transform.Rotate(new Vector3(-90, 0));
            tmp.transform.SetParent(parent);
            boxes.Add(tmp);
            //nrOfBoxes++;
        }
        else if (id > 94 && id <= 96)
        {
            GameObject tmp = Instantiate(boxTemplates[(int)BoxEnum.BoxBrick], position, Quaternion.identity);
            tmp.transform.Rotate(new Vector3(-90, 0));
            tmp.transform.SetParent(parent);
            boxes.Add(tmp);
            nrOfBoxes++;
        }
        else if (id > 96 && id <= 98)
        {
            GameObject tmp = Instantiate(boxTemplates[(int)BoxEnum.BoxBomb], position, Quaternion.identity);
            tmp.transform.Rotate(new Vector3(-90, 0));
            tmp.transform.SetParent(parent);
            boxes.Add(tmp);
            nrOfBoxes++;
        }
        else if(id > 98 && id <= 100)
        {
            GameObject tmp = Instantiate(boxTemplates[(int)BoxEnum.BoxHeal], position, Quaternion.identity);
            tmp.transform.Rotate(new Vector3(-90, 0));
            tmp.transform.SetParent(parent);
            boxes.Add(tmp);
            nrOfBoxes++;
        }
    }

    void InstantiateAllBoxes()
    {
        for (int i = 0; i < boxes.Count; i++)
        {
            Destroy(boxes[i]);
        }
        boxes.Clear();
        nrOfBoxes = 0;
        for (int i = 0; i < 14; i++)
        {
            for (int y = 0; y < 13; y++)
            {
                float distFactorX = 0.0f;
                float distFactorY = 0.0f;
                float startOffsetX = -1.0f;
                float startOffsetY = 2.0f;
                Vector3 pos = new Vector3(i + i * distFactorX - startOffsetX, y*0.5f + y * distFactorY - startOffsetY, 0.0f);
                InstantiateBox(pos);
            }
        }
    }

    public List<GameObject> GetBoxes()
    {
        return boxes;
    }

    public void Reset()
    {
        InstantiateAllBoxes();
    }

    public void LowerNrOfBoxes()
    {
        nrOfBoxes--;
        if(nrOfBoxes <= 0)
        {
            //Debug.Log("no boxes!");
            gamehandler.AdvanceLevel();
        }
    }

    public void RemoveBox(GameObject box, bool lowerBoxCount)
    {
        if (!boxes.Remove(box)){
            Debug.Log("Failed to remove box: " + box.name);
        }
        Destroy(box);

        if(lowerBoxCount)
            LowerNrOfBoxes();
    }
}
