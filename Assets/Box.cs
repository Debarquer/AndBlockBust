using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {

    protected float health;
    public float dmgCDMax;
    public float dmgCDCurr;
    public BoxHandler boxhandler;

    public bool lowerBoxCount;

	// Use this for initialization
	void Start () {
        health = -1;

        dmgCDMax = 0.5f;
        dmgCDCurr = 0.0f;

        boxhandler = GameObject.Find("BoxHandler").GetComponent<BoxHandler>();
    }

    // Update is called once per frame
    void Update () {
        dmgCDCurr += Time.deltaTime;
	}

    virtual public bool Hit(int damage, out bool damageBall)
    {
        Debug.Log("Activated the virtual hit function");
        damageBall = false;
        return false;
    }
}
