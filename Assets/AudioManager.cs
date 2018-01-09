using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    AudioSource audiosource;

    public AudioClip ButtonClick;
    public AudioClip AdvanceLevel;
    public AudioClip LoseLife;
    public AudioClip LevelUp;

    //block sounds
    public AudioClip Block;
    public AudioClip BlockBomb;
    public AudioClip BlockBrick;
    public AudioClip IncreaseHealth;

    public AudioClip LevelUpLoop;

    // Use this for initialization
    void Start () {
        audiosource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayButtonClick()
    {
        audiosource.PlayOneShot(ButtonClick);
    }

    public void PlayAdvanceLevel()
    {
        audiosource.PlayOneShot(AdvanceLevel);
    }

    public void PlayLoseLife()
    {
        audiosource.PlayOneShot(LoseLife);
    }

    public void PlayLevelUp()
    {
        audiosource.PlayOneShot(LevelUp);
    }

    //Blocks
    public void PlayBlock()
    {
        audiosource.PlayOneShot(Block);
    }

    public void PlayBlockBomb()
    {
        audiosource.PlayOneShot(BlockBomb);
    }

    public void PlayBlockBrick()
    {
        audiosource.PlayOneShot(BlockBrick);
    }

    public void PlayIncreaseHealth()
    {
        audiosource.PlayOneShot(IncreaseHealth);
    }

    public void StartLevelUpLoop()
    {
        //if (!audiosource.loop)
        //{
        //    audiosource.loop = true;
        //    audiosource.clip = LevelUpLoop;
        //    audiosource.
        //    audiosource.Play();
        //}
    }

    public void StopLevelUpLoop()
    {
        //audiosource.loop = false;
        //audiosource.Stop();
    }
}
