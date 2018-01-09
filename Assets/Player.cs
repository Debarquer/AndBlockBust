using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public GameHandler gamehandler;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

        
    }

    private void FixedUpdate()
    {
        if (gamehandler.gamestate != GameHandler.GameState.Playing)
            return;

        float speed = Time.deltaTime * 7;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(-speed, 0, 0);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(speed, 0, 0);
        }

        if(Mathf.Abs(Input.acceleration.x) > 0.05f)
        {
            if(Input.acceleration.x > 0)
            {
                transform.Translate(new Vector3(Input.acceleration.x - 0.05f, 0.0f, 0.0f));
            }
            else
            {
                transform.Translate(new Vector3(Input.acceleration.x + 0.05f, 0.0f, 0.0f));
            }
        }
            

        //Debug.Log(this.transform.position.x);
        if (this.transform.position.x > 16.0f)
        {
            this.transform.position = new Vector3(16.0f, transform.position.y, transform.position.z);
        }
        else if (this.transform.position.x < 0.1f)
        {
            this.transform.position = new Vector3(0.1f, transform.position.y, transform.position.z);
        }
    }

    public void Reset()
    {
        transform.position = new Vector3(8.5f, -4.0f, 0.0f);
    }
}
