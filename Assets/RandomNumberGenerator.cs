using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RandomNumberGenerator : MonoBehaviour {

    System.Random random;

	// Use this for initialization
	void Start () {
        random = new System.Random();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int GenerateRandomNumber(int maxValue)
    {
        return random.Next(maxValue);
    }
}
